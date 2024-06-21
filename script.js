document.addEventListener('DOMContentLoaded', function() {
    var touchstartX = 0;
    var touchstartY = 0;
    var touchendX = 0;
    var touchendY = 0;
    var gesture = '';
    var tapCount = 0;
    var tapTimeout;
    var twoFingerTapCount = 0;
    var twoFingerTapTimeout;
  
    function detectGesture() {
      var deltaX = touchendX - touchstartX;
      var deltaY = touchendY - touchstartY;
  
      if (Math.abs(deltaX) > Math.abs(deltaY)) {
        // Horizontal swipe
        if (deltaX > 30) {
          gesture = 'Right';
        } else if (deltaX < -30) {
          gesture = 'Left';
        }
      } else {
        // Vertical swipe
        if (deltaY < -30) {
          gesture = 'Up';
        }
      }
    }
  
    document.addEventListener('touchstart', function(event) {
      if (event.touches.length === 1) {
        touchstartX = event.touches[0].pageX;
        touchstartY = event.touches[0].pageY;
      } else if (event.touches.length === 2) {
        touchstartX = (event.touches[0].pageX + event.touches[1].pageX) / 2;
        touchstartY = (event.touches[0].pageY + event.touches[1].pageY) / 2;
      }
    }, false);
  
    document.addEventListener('touchmove', function(event) {
      if (event.touches.length === 1 || event.touches.length === 2) {
        event.preventDefault();
      }
    }, false);
  
    document.addEventListener('touchend', function(event) {
      if (event.touches.length === 0 && event.changedTouches.length === 1) {
        touchendX = event.changedTouches[0].pageX;
        touchendY = event.changedTouches[0].pageY;
        detectGesture();
  
        if (gesture) {
          document.getElementById("Debug").innerHTML = gesture;
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
            }
            document.getElementById("Debug").innerHTML = tapGesture;
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
          document.getElementById("Debug").innerHTML = twoFingerTapGesture;
          twoFingerTapCount = 0;
        }, 300);
      }
    }, false);
  });
  