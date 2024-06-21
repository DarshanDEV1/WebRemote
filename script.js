document.addEventListener('DOMContentLoaded', function() {
    var touchArea = document.getElementById('touchArea');
    var tapSound = document.getElementById('tapSound');
    var swipeSound = document.getElementById('swipeSound');
    var debug = document.getElementById('Debug');
    var modeToggle = document.getElementById('modeToggle');
    var serverUrlInput = document.getElementById('serverUrl');
    var connectBtn = document.getElementById('connectBtn');

    var socket;
    var touchstartX = 0;
    var touchstartY = 0;
    var touchendX = 0;
    var touchendY = 0;
    var gesture = '';
    var tapCount = 0;
    var tapTimeout;
    var mouseMode = true;
    var beepQueue = [];
    var isPlaying = false;

    function detectGesture() {
        var deltaX = touchendX - touchstartX;
        var deltaY = touchendY - touchstartY;

        if (Math.abs(deltaX) > Math.abs(deltaY)) {
            if (deltaX > 30) {
                gesture = 'Right';
                swipeSound.play();
                emitAction('swipe_right');
            } else if (deltaX < -30) {
                gesture = 'Left';
                swipeSound.play();
                emitAction('swipe_left');
            }
        } else {
            if (deltaY < -30) {
                gesture = 'Up';
                swipeSound.play();
                emitAction('swipe_up');
            } else if (deltaY > 30) {
                gesture = 'Down';
                swipeSound.play();
                emitAction('swipe_down');
            }
        }
    }

    function playBeep(count) {
        for (var i = 0; i < count; i++) {
            beepQueue.push(tapSound);
        }
        if (!isPlaying) {
            playNextBeep();
        }
    }

    function playNextBeep() {
        if (beepQueue.length > 0) {
            isPlaying = true;
            var sound = beepQueue.shift();
            sound.play();
            sound.onended = function() {
                playNextBeep();
            };
        } else {
            isPlaying = false;
        }
    }

    function updateTheme() {
        if (mouseMode) {
            document.body.style.backgroundColor = '#f0f0f0';
            document.body.style.color = '#000';
            touchArea.style.backgroundColor = '#fff';
            touchArea.style.color = '#000';
        } else {
            document.body.style.backgroundColor = '#333';
            document.body.style.color = '#fff';
            touchArea.style.backgroundColor = '#444';
            touchArea.style.color = '#fff';
        }
    }

    function emitAction(action) {
        if (socket) {
            socket.emit('action_signal', { signal: action });
        }
    }

    modeToggle.addEventListener('click', function() {
        mouseMode = !mouseMode;
        modeToggle.classList.toggle('on');
        updateTheme();
        debug.innerHTML = mouseMode ? 'Mouse Mode' : 'Remote Mode';
    });

    touchArea.addEventListener('touchstart', function(event) {
        event.preventDefault();
        if (event.touches.length === 1) {
            touchstartX = event.touches[0].pageX;
            touchstartY = event.touches[0].pageY;
        }
    }, false);

    touchArea.addEventListener('touchmove', function(event) {
        event.preventDefault();
        if (mouseMode && event.touches.length === 1) {
            var touch = event.touches[0];
            var x = touch.pageX;
            var y = touch.pageY;
            debug.innerHTML = 'Coordinates: (' + x + ', ' + y + ')';
            emitAction(`mouse_move:${x},${y}`);
        }
    }, false);

    touchArea.addEventListener('touchend', function(event) {
        event.preventDefault();
        if (event.touches.length === 0 && event.changedTouches.length === 1) {
            touchendX = event.changedTouches[0].pageX;
            touchendY = event.changedTouches[0].pageY;
            detectGesture();

            if (gesture) {
                debug.innerHTML = gesture;
                gesture = '';
            } else {
                tapCount++;
                clearTimeout(tapTimeout);
                tapTimeout = setTimeout(function() {
                    var tapGesture = '';
                    switch (tapCount) {
                        case 1:
                            tapGesture = 'Single Tap';
                            playBeep(1);
                            emitAction('single_tap');
                            break;
                        case 2:
                            tapGesture = 'Double Tap';
                            playBeep(2);
                            emitAction('double_tap');
                            break;
                        case 3:
                            tapGesture = 'Triple Tap';
                            playBeep(3);
                            emitAction('triple_tap');
                            break;
                        case 4:
                            tapGesture = 'Four Taps';
                            playBeep(4);
                            emitAction('four_taps');
                            break;
                    }
                    debug.innerHTML = tapGesture;
                    tapCount = 0;
                }, 300);
            }
        }
    }, false);

    connectBtn.addEventListener('click', function() {
        var serverUrl = serverUrlInput.value.trim();
        if (serverUrl) {
            socket = io(serverUrl);
            socket.on('connect', function() {
                debug.innerHTML = 'Connected to server';
            });
            socket.on('disconnect', function() {
                debug.innerHTML = 'Disconnected from server';
                socket = null;
            });
        }
    });

    updateTheme();
});
