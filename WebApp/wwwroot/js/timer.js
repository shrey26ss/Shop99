class ShowJsTimer {
    constructor(elem, timeInMinutes) {
        this._elem = elem;
        this._timeInMinutes = timeInMinutes === undefined ? 5 : timeInMinutes;
    }
    startTimer(onClose) {
        let currentTime = new Date();
        currentTime.setMinutes(currentTime.getMinutes() + this._timeInMinutes);
        let __this = this;
        __this._st = setInterval(function () {
            let now = new Date().getTime();
            let diff = currentTime - now;
            var minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((diff % (1000 * 60)) / 1000);
            if (minutes > -1 && seconds > -1) {
                __this._elem.innerHTML = minutes.toString().padStart(2, 0) + ":" + seconds.toString().padStart(2, 0);
            }
            else {
                __this._elem.innerHTML = "00:00";
            }
            if (diff <= 0) {
                clearInterval(__this._st);
                if (onClose !== undefined)
                    onClose();
            }
        }, 1000);
    }
    stopTimer(f) {
        if (this._st === undefined)
            return true;
        clearInterval(this._st);
        if (f === undefined)
            return true;
        f();
    }
}