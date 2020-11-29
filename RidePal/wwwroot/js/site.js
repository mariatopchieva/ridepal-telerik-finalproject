// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//statistics animation
$('.counter-count').each(function () {
    $(this).prop('Counter', 0).animate({
        Counter: $(this).text()
    }, {
        duration: 5000,
        easing: 'swing',
        step: function (now) {
            $(this).text(Math.ceil(now));
        }
    });
});

//card animation
$(document).ready(function () {

    $('.card').delay(1800).queue(function (next) {
        $(this).removeClass('hover');
        $('a.hover').removeClass('hover');
        next();
    });
});

//Create playlist form Slider enable
$('#inputMetal').click(function () {
    var checked = this.checked;
    $('#inputMetalSlider').each(function () {
        $(this).prop('disabled', !checked);
    });
});

$('#inputRock').click(function () {
    var checked = this.checked;
    $('#inputRockSlider').each(function () {
        $(this).prop('disabled', !checked);
    });
});

$('#inputPop').click(function () {
    var checked = this.checked;
    $('#inputPopSlider').each(function () {
        $(this).prop('disabled', !checked);
    });
});

$('#inputJazz').click(function () {
    var checked = this.checked;
    $('#inputJazzSlider').each(function () {
        $(this).prop('disabled', !checked);
    });
});

//sliders numbers
$(document).ready(
    function () {

        const $valueSpan = $('#metalSpan');
        const $value = $('#inputMetalSlider');
        $valueSpan.html($value.val());
        $value.on('input change', () => {

            $valueSpan.html($value.val());
        });
    });

$(document).ready(
    function () {

        const $valueSpan = $('#rockSpan');
        const $value = $('#inputRockSlider');
        $valueSpan.html($value.val());
        $value.on('input change', () => {

            $valueSpan.html($value.val());
        });
    });

$(document).ready(
    function () {

        const $valueSpan = $('#popSpan');
        const $value = $('#inputPopSlider');
        $valueSpan.html($value.val());
        $value.on('input change', () => {

            $valueSpan.html($value.val());
        });
    });

$(document).ready(
    function () {

        const $valueSpan = $('#jazzSpan');
        const $value = $('#inputJazzSlider');
        $valueSpan.html($value.val());
        $value.on('input change', () => {

            $valueSpan.html($value.val());
        });
    });

//slider duration
$(document).ready(
    function () {

        const $valueSpan = $('#durationSpan');
        const $value = $('#durationSlider');
        $valueSpan.html($value.val());
        $value.on('input change', () => {

            $valueSpan.html($value.val());
        });
    });