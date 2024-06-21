var touchstartX = 0;
var touchstartY = 0;
var touchendX = 0;
var touchendY = 0;

document.addEventListener('touchstart', function(event) {
    touchstartX = event.touches[0].pageX;
    touchstartY = event.touches[0].pageY;
}, false);

document.addEventListener('touchmove', function(event) {
    event.preventDefault();
}, false);

document.addEventListener('touchend', function(event) {
    touchendX = event.changedTouches[0].pageX;
    touchendY = event.changedTouches[0].pageY;
    handleSwipe();
}, false);

function handleSwipe() {
    if (touchendX < touchstartX) {
        // Swipe left
        console.log('Swiped left');
        document.getElementById("Debug").innerHTML = "Swiped Left";
    } else if (touchendX > touchstartX) {
        // Swipe right
        console.log('Swiped right');
        document.getElementById("Debug").innerHTML = "Swiped Right";
    }
}