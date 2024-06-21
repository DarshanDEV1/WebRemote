document.addEventListener('DOMContentLoaded', function() {
    var touchArea = document.getElementById('touchArea');
    var debug = document.getElementById('Debug');

    var touchstartX = 0;
    var touchstartY = 0;
    var touchendX = 0;
    var touchendY = 0;
    var gesture = '';
    var tapCount = 0;
    var tapTimeout;
    var twoFingerTapCount = 0;
    var twoFingerTapTimeout;
    var initialPinchDistance = null;

    function detectGesture() {
        var deltaX = touchendX - touchstartX;
        var deltaY = touchendY - touchstartY;

        if (Math.abs(deltaX) > Math.abs(deltaY)) {
            if (deltaX > 30) {
                gesture = 'Right';
            } else if (deltaX < -30) {
                gesture = 'Left';
            }
        } else {
            if (deltaY < -30) {
                gesture = 'Up';
            }
        }
    }

    function getDistance(touches) {
        var dx = touches[0].pageX - touches[1].pageX;
        var dy = touches[0].pageY - touches[1].pageY;
        return Math.sqrt(dx * dx + dy * dy);
    }

    touchArea.addEventListener('touchstart', function(event) {
        if (event.touches.length === 1) {
            touchstartX = event.touches[0].pageX;
            touchstartY = event.touches[0].pageY;
        } else if (event.touches.length === 2) {
            initialPinchDistance = getDistance(event.touches);
            touchstartX = (event.touches[0].pageX + event.touches[1].pageX) / 2;
            touchstartY = (event.touches[0].pageY + event.touches[1].pageY) / 2;
        }
    }, false);

    touchArea.addEventListener('touchmove', function(event) {
        if (event.touches.length === 1 || event.touches.length === 2) {
            event.preventDefault();
        }

        if (event.touches.length === 2) {
            var currentPinchDistance = getDistance(event.touches);
            if (initialPinchDistance) {
                if (currentPinchDistance > initialPinchDistance) {
                    debug.innerHTML = 'Zoom Out';
                } else {
                    debug.innerHTML = 'Zoom In';
                }
            }
            initialPinchDistance = currentPinchDistance;
        }
    }, false);

    touchArea.addEventListener('touchend', function(event) {
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
                            break;
                        case 2:
                            tapGesture = 'Double Tap';
                            break;
                        case 3:
                            tapGesture = 'Triple Tap';
                            break;
                        case 4:
                            tapGesture = "Four Taps";
                            break;
                    }
                    debug.innerHTML = tapGesture;
                    tapCount = 0;
                }, 300);
            }
        } else if (event.touches.length === 0 && event.changedTouches.length === 2) {
            twoFingerTapCount++;
            clearTimeout(twoFingerTapTimeout);
            twoFingerTapTimeout = setTimeout(function() {
                var twoFingerTapGesture = '';
                switch (twoFingerTapCount) {
                    case 1:
                        twoFingerTapGesture = 'Two Finger Single Tap';
                        break;
                    case 2:
                        twoFingerTapGesture = 'Two Finger Double Tap';
                        break;
                    case 3:
                        twoFingerTapGesture = 'Two Finger Triple Tap';
                        break;
                }
                debug.innerHTML = twoFingerTapGesture;
                twoFingerTapCount = 0;
            }, 300);
        }
    }, false);
});
