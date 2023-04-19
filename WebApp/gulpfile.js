/// <binding />
//"use strict";
var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    babel = require('gulp-babel');

var webroot = "./wwwroot/";

var paths = {
    js: webroot + "js/**/*.js",
    minJs: webroot + "js/**/*.min.js",
    css: webroot + "css/**/*.css",
    minCss: webroot + "css/**/*.min.css",
    concatJsDest: webroot + "js/publishjs/*.js",
    concatCssDest: webroot + "css/site.min.css"
};

var webpackages = {
    "requirejs": { "bin/*": "bin/" },
    "jquery": { "dist/*": "dist/" },
    "jquery-ui": { "dist/*": "dist/" },
    "toastr": { "build/*": "build/" },
    "bootstrap": { "dist/**/*": "dist/" },
    "babel": { "dist/*": "dist/" },
    "datatables": { "media/**/*": "dist/" },
    "datatables-bootstrap": { "js/*": "dist/" }
}

gulp.task("publish:plugins", async function () {
    var streams = [];
    for (var package in webpackages) {
        for (var item in webpackages[package]) {
            streams.push(gulp.src("node_modules/" + package + "/" + item)
                .pipe(gulp.dest(webroot + "lib/" + package + "/" + webpackages[package][item])));
        }
    } 
})

var destPath = './wwwroot/build';

gulp.task("bundleJS", async function () {
    gulp.src([
        'node_modules/jquery/dist/jquery.js',
        'node_modules/jquery-ui/dist/jquery-ui.js',
        'node_modules/bootstrap/dist/js/bootstrap.bundle.js',
        'node_modules/toastr/toastr.js',
        'wwwroot/lib/datatables/dist/js/jquery.datatables.min.js',
        'wwwroot/js/coreLib.js',
    ])
        .pipe(babel({ compact: false, presets: ['@babel/preset-env'] }))
        .pipe(uglify())
        .pipe(concat('bundleJS.js'))
        .pipe(gulp.dest(destPath));
})

//gulp.task("min:js", async function () {
//    let files = [
//        {
//            plugin: 'jquery',
//            source: 'node_modules/jquery/dist/jquery.js',
//            disName: 'jquery.min.js'
//        },
//        {
//            plugin: 'jqueryUI',
//            source: 'node_modules/jquery-ui/dist/jquery-ui.js',
//            disName: 'jquery-ui.min.js'
//        },
//        {
//            plugin: 'bootstrap',
//            source: 'node_modules/bootstrap/dist/js/bootstrap.bundle.js',
//            disName: 'bootstrap.min.js'
//        }
//        ,
//        {
//            plugin: 'toastr',
//            source: 'node_modules/toastr/toastr.js',
//            disName: 'toastr.min.js'
//        }
//    ];
//    for (let i = 0; i < files.length; i++) {
//        let __f = files[i];
//        gulp.src(__f.source)
//            //.pipe(concat('bundleJS'))
//            .pipe(babel({ compact: false, presets: ['@babel/preset-env'] }))
//            //.pipe(gulp.dest(destPPath))
//            .pipe(uglify())
//            .pipe(concat(__f.disName))
//            .pipe(gulp.dest(destPath));
//    }
//})

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("build", gulp.series("clean:js", "clean:css", "publish:plugins", "bundleJS"));