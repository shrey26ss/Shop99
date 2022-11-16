




























//function Graph(canvas) {
//    var ratio = window.devicePixelRatio || 1;

//    this.h = canvas.height;
//    this.w = canvas.width;

//    canvas.width *= ratio;
//    canvas.height *= ratio;
//    canvas.style.width = this.w + "px";
//    canvas.style.height = this.h + "px";

//    this.ctx = canvas.getContext("2d");
//    this.ctx.scale(ratio, ratio);
//    this.data = {};
//    this.keys = {};

//    var padding = 48;
//    var offset = 10;
//    var points = [];
//    var vstep, ystep, xstep;

//    this.format = function (v) {
//        return Number(v).
//            toFixed(2).
//            toLocaleString();
//    };

//    this.reset = function (ctx) {
//        points = [];
//        ctx.beginPath();
//        ctx.clearRect(0, 0, this.w, this.h);
//        ctx.lineWidth = 1;
//        ctx.textBaseline = "middle";
//        ctx.font = "300 12px Roboto Condensed";
//        ctx.strokeStyle = "#ccc";
//    };

//    this.drawGrid = function (ctx) {
//        ctx.fillStyle = "#333";
//        ctx.textAlign = "right";
//        for (var i = 0; i < vstep; i += 1) {
//            if (window.CP.shouldStopExecution(0)) break;
//            var target = i * ystep + offset;
//            var text = this.data.row.max - i * this.data.row.step;
//            ctx.moveTo(padding + offset, target);
//            ctx.fillText(this.format(text), padding, target);
//            ctx.lineTo(this.w, target);
//        } window.CP.exitedLoop(0);
//        ctx.stroke();
//    };

//    this.computePoints = function () {
//        for (var j = 0; j < this.keys.length; j += 1) {
//            if (window.CP.shouldStopExecution(1)) break;
//            var x = (padding * 2 + j * xstep) * 0.92;
//            var y = Math.round(
//                this.data.col[this.keys[j]] * this.h / this.data.row.max);

//            y =
//                y - Math.round(this.data.col[this.keys[j]] / this.data.row.max * ystep);
//            y = this.h - y - ystep + offset;
//            points.push({ x: x, y: y });
//        } window.CP.exitedLoop(1);
//    };

//    this.drawPoints = function (ctx) {
//        ctx.beginPath();
//        ctx.fillStyle = "#0083ba";
//        ctx.strokeStyle = "#fff";
//        ctx.lineWidth = 5;
//        ctx.textAlign = "center";
//        for (var j = 0; j < points.length; j += 1) {
//            if (window.CP.shouldStopExecution(2)) break;
//            var point = points[j];
//            var x = (padding * 2 + j * xstep) * 0.92;
//            var y = this.h - vstep - offset;
//            ctx.fillText(this.keys[j], x, y);
//            ctx.moveTo(point.x, point.y);
//            ctx.closePath();
//            ctx.arc(point.x, point.y, 4, 0, 2 * Math.PI);
//        } window.CP.exitedLoop(2);
//        ctx.stroke();
//        ctx.fill();
//    };

//    this.drawBackground = function (ctx) {
//        ctx.beginPath();
//        ctx.fillStyle = "rgba(0, 200, 255, .2)";
//        var y = this.h - ystep + offset / 2;
//        ctx.moveTo(points[0].x, y);
//        for (var k = 0; k < points.length; k += 1) {
//            if (window.CP.shouldStopExecution(3)) break;
//            var point = points[k];
//            ctx.lineTo(point.x, point.y);
//        } window.CP.exitedLoop(3);
//        ctx.lineTo(points[points.length - 1].x, y);
//        ctx.fill();
//    };

//    this.drawLines = function (ctx) {
//        ctx.beginPath();
//        ctx.strokeStyle = "#0083ba";
//        ctx.lineWidth = 1;
//        ctx.fillStyle = "#fff";
//        ctx.moveTo(points[0].x, points[0].y);
//        for (var k = 0; k < points.length; k += 1) {
//            if (window.CP.shouldStopExecution(4)) break;
//            var point = points[k];
//            ctx.lineTo(point.x, point.y);
//        } window.CP.exitedLoop(4);
//        ctx.stroke();
//    };

//    this.draw = function (data) {
//        this.reset(this.ctx);
//        this.data = data;
//        this.keys = Object.keys(data.col);
//        vstep = Math.round(data.row.max / data.row.step) + 1;
//        ystep = Math.round(this.h / vstep);
//        xstep = Math.round(this.w / this.keys.length);
//        this.drawGrid(this.ctx);
//        this.computePoints(this.ctx);
//        this.drawLines(this.ctx);
//        this.drawBackground(this.ctx);
//        this.drawPoints(this.ctx);
//    };
//}

//function data(mins, maxs, steps, cData) {
//    let c = {};
//    let r = { min: mins, max: maxs, step: steps };
//    for (var i = 0; i < cData.length; i++) {
//        if (window.CP.shouldStopExecution(5)) break;
//        c[cData[i].entryOn] = cData[i].successTransaction;
//    } window.CP.exitedLoop(5);
//    return { col: c, row: r };
//}

//function redraw(mins, maxs, steps, cData) { g.draw(data(mins, maxs, steps, cData)); }
//var g = new Graph(document.querySelector("#c"));