document.addEventListener('DOMContentLoaded', function() {
    var touchstartX = 0;
    var touchstartY = 0;
    var gesture = '';
  
    document.addEventListener('touchstart', function(event) {
      touchstartX = event.touches[0].pageX;
      touchstartY = event.touches[0].pageY;
      gesture = ''; // Reset gesture on new touchstart
    }, false);
  
    document.addEventListener('touchmove', function(event) {
      event.preventDefault();
      var touchmoveX = event.touches[0].pageX;
      var touchmoveY = event.touches[0].pageY;
  
      // Simplified gesture detection (A-Z)
      if (touchmoveX > touchstartX && touchmoveY < touchstartY) {
        gesture += 'UR'; // Up and right
      } else if (touchmoveX < touchstartX && touchmoveY < touchstartY) {
        gesture += 'UL'; // Up and left
      } else if (touchmoveX > touchstartX && touchmoveY > touchstartY) {
        gesture += 'DR'; // Down and right
      } else if (touchmoveX < touchstartX && touchmoveY > touchstartY) {
        gesture += 'DL'; // Down and left
      } else if (touchmoveX > touchstartX) {
        gesture += 'R'; // Right
      } else if (touchmoveX < touchstartX) {
        gesture += 'L'; // Left
      } else if (touchmoveY > touchstartY) {
        gesture += 'D'; // Down
      } else if (touchmoveY < touchstartY) {
        gesture += 'U'; // Up
      }
  
      // Update the start coordinates for the next move
      touchstartX = touchmoveX;
      touchstartY = touchmoveY;
    }, false);
  
    document.addEventListener('touchend', function(event) {
      var touchendX = event.changedTouches[0].pageX;
      var touchendY = event.changedTouches[0].pageY;
  
      // Map gesture to alphabet (A-Z)
      switch (gesture) {
        case 'RDLU':
          document.getElementById("Debug").innerHTML = 'A';
          break;
        case 'RUL':
          document.getElementById("Debug").innerHTML = 'B';
          break;
        case 'RDL':
          document.getElementById("Debug").innerHTML = 'C';
          break;
        case 'LURD':
          document.getElementById("Debug").innerHTML = 'D';
          break;
        case 'LR':
          document.getElementById("Debug").innerHTML = 'E';
          break;
        // Add more cases for each alphabet letter (F-Z)
        default:
          document.getElementById("Debug").innerHTML = 'Unknown gesture: ' + gesture;
      }
    }, false);
  });
  