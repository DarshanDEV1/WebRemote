document.addEventListener('DOMContentLoaded', function() {
    var touchstartX = 0;
    var touchstartY = 0;
    var touchendX = 0;
    var touchendY = 0;
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
        gesture += 'U'; // Up and right
      } else if (touchmoveX < touchstartX && touchmoveY < touchstartY) {
        gesture += 'D'; // Down and left
      } else if (touchmoveX > touchstartX && touchmoveY > touchstartY) {
        gesture += 'R'; // Right and down
      } else if (touchmoveX < touchstartX && touchmoveY > touchstartY) {
        gesture += 'L'; // Left and up
      } else if (touchmoveX > touchstartX) {
        gesture += 'R'; // Right
      } else if (touchmoveX < touchstartX) {
        gesture += 'L'; // Left
      } else if (touchmoveY > touchstartY) {
        gesture += 'D'; // Down
      } else if (touchmoveY < touchstartY) {
        gesture += 'U'; // Up
      }
    }, false);
  
    document.addEventListener('touchend', function(event) {
      touchendX = event.changedTouches[0].pageX;
      touchendY = event.changedTouches[0].pageY;
  
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
        // Add more cases for each alphabet letter (D-Z)
        default:
          document.getElementById("Debug").innerHTML = 'Unknown gesture';
      }
    }, false);
  });