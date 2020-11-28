var button = document.getElementById("playAudio");
var audio = document.getElementById("myAudio");

button.addEventListener("click", function () {
    if (audio.paused) {
        audio.play();
        button.innerHTML = "Pause";
    } else {
        audio.pause();
        button.innerHTML = "Play";
    }
});