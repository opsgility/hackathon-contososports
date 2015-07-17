function padZeros(n) {
    if (n === 0) {
        return "00";
    }
    return (n < 10) ? ("0" + n) : n;
}

function countUpDownTimer(kickOffMillisecondsSinceEpoch) {
    var end = new Date(kickOffMillisecondsSinceEpoch);

    var _second = 1000;
    var _minute = _second * 60;
    var _hour = _minute * 60;
    var _day = _hour * 24;

    function showRemaining() {
        var now = new Date();
        var distance = end - now;
        var countingDown = true;

        if (distance < 0) {
            countingDown = false;
            distance *= -1;
        }

        var days = Math.floor(distance / _day);
        var hours = Math.floor((distance % _day) / _hour);
        var minutes = Math.floor((distance % _hour) / _minute);
        var seconds = Math.floor((distance % _minute) / _second);

        var timeLabel = (days > 1) ? days + ' days' : padZeros(hours) + ':' + padZeros(minutes) + ':' + padZeros(seconds);

        $('#countdownTimer p').text((countingDown ? 'T-' : '') + timeLabel);
    }

    setInterval(showRemaining, 1000);
}