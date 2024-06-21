document.addEventListener('DOMContentLoaded', function() {
    var touchstartX = 0;
    var touchstartY = 0;
    var touchendX = 0;
    var touchendY = 0;
    var gesture = '';
    var minDistance = 30; // Minimum distance to register a direction change
  
    document.addEventListener('touchstart', function(event) {
      touchstartX = event.touches[0].pageX;
      touchstartY = event.touches[0].pageY;
      gesture = ''; // Reset gesture on new touchstart
    }, false);
  
    document.addEventListener('touchmove', function(event) {
      event.preventDefault();
      var touchmoveX = event.touches[0].pageX;
      var touchmoveY = event.touches[0].pageY;
      var deltaX = touchmoveX - touchstartX;
      var deltaY = touchmoveY - touchstartY;
  
      // Detect significant movements only
      if (Math.abs(deltaX) > minDistance || Math.abs(deltaY) > minDistance) {
        if (Math.abs(deltaX) > Math.abs(deltaY)) {
          // Horizontal movement
          if (deltaX > 0) {
            gesture += 'R'; // Right
          } else {
            gesture += 'L'; // Left
          }
        } else {
          // Vertical movement
          if (deltaY > 0) {
            gesture += 'D'; // Down
          } else {
            gesture += 'U'; // Up
          }
        }
  
        // Update the start coordinates for the next segment
        touchstartX = touchmoveX;
        touchstartY = touchmoveY;
      }
    }, false);
  
    document.addEventListener('touchend', function(event) {
      touchendX = event.changedTouches[0].pageX;
      touchendY = event.changedTouches[0].pageY;
  
      // Map gesture to alphabet (A-Z)
      var detectedLetter = '';
  
      switch (gesture) {
        case 'RDLU':
          detectedLetter = 'A';
          break;
        case 'RUL':
          detectedLetter = 'B';
          break;
        case 'RDL':
          detectedLetter = 'C';
          break;
        case 'LURD':
          detectedLetter = 'D';
          break;
        case 'LR':
          detectedLetter = 'E';
          break;
        // Add more cases for each alphabet letter (F-Z)
        default:
          detectedLetter = 'Unknown gesture: ' + gesture;
      }
  
      document.getElementById("Debug").innerHTML = detectedLetter;
    }, false);
  });
  