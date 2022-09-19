/// <binding ProjectOpened='watch, build' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

// Плагины и конфиги
var gulp = require('gulp'),
    concat = require("gulp-concat"),
    csso = require("gulp-csso"),
    uglify = require("gulp-uglify"),
    del = require("del"),
    image = require("gulp-imagemin"),
    sourcemaps = require("gulp-sourcemaps"),
    _if = require("gulp-if"),
    config = require("./gulpconfig.json"),
    fs = require('fs');

// На Pre-Build фронтенда должно быть повешено событие:
// if $(ConfigurationName) == Debug (echo $(ConfigurationName)>"$(ProjectDir)BuildConfiguration.txt") ELSE (echo off>"$(ProjectDir)BuildConfiguration.txt")
// Для Debug конфигурации файлы app не минифицируются
var buildConfiguration = fs.readFileSync("BuildConfiguration.txt", 'ascii').split(/[\n\r] /)[0];
// _if условие должно быть truthy или falsy 
var isDebug = !!buildConfiguration;
var paths = config.paths;

/*
config.cssFiles - необходимые стили
config.resourceFiles - библиотеки
config.appFiles - файлы приложения
*/


//Задачи по очистке каталогов. cb - callback'и нужны для возможности синхронного запуска задач
gulp.task("clean:app", function (cb) {
    del(paths.concatJsDest, cb);
    cb();
});

gulp.task("clean:res", function (cb) {
    del(paths.concatResDest, cb);
    cb();
});

gulp.task("clean:css", function (cb) {
    del(paths.concatCssDest, cb);
    cb();
});

gulp.task("clean:cssRS", function (cb) {
    del(paths.concatCssRSDest, cb);
    cb();
});

gulp.task("clean:img", function (cb) {
    del(paths.imagesDestPaths);
    cb();
});

gulp.task("clean:font", function (cb) {
    del(paths.fontsDestPath + "*", cb);
    cb();
});

// Задачи по объединению и минификации js-файлов приложения, стилей и js-файлов ресурсов/библиотек
gulp.task("min:app", function () {
    return gulp.src(config.appFiles, { nounique: false, nosort: true })
        .pipe(concat(paths.concatJsDest))
        .pipe(_if(!isDebug, uglify().on('error', function (e) {
            console.error(e); // вывод ошибки минификации в лог
            throw e; // Для завершение с ошибкой
        })))
        .pipe(gulp.dest("."));
});

gulp.task("min:resource", function () {
    return gulp.src(config.resourceFiles)
        .pipe(concat(paths.concatResDest))
        //.pipe(_if(!isDebug, uglify()))
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src(config.cssFiles)
        .pipe(concat(paths.concatCssDest))
        .pipe(csso())
        .pipe(gulp.dest("."));
});

gulp.task("min:cssRS", function () {
    return gulp.src(config.cssRSFiles)
        .pipe(concat(paths.concatCssRSDest))
        .pipe(csso())
        .pipe(gulp.dest("."));
});

// Копирование и сжатие изображений
gulp.task("images:img", function () {
    return gulp.src(paths.imgBase + "**/*", { base: paths.imgBase }) // base - для сохранения структуры каталогов при копировании
        .pipe(image())
        .pipe(gulp.dest(paths.imagesPath));
});

// Копирование и сжатие изображений
gulp.task("images:moduleimg", function () {
    return gulp.src(paths.modImgBase + "**/*", { base: paths.modImgBase }) // base - для сохранения структуры каталогов при копировании
        .pipe(image())
        .pipe(gulp.dest(paths.imagesPath));
});

//Копирование шрифтов 
gulp.task("fonts", function () {
    return gulp.src(paths.fontsBase)
        .pipe(gulp.dest(paths.fontsDestPath));
});

// Очистка всех папок
gulp.task("clean", gulp.series("clean:app", "clean:res", "clean:css", "clean:img", "clean:font"), function (callback) {
    callback();
});

// Сборка
gulp.task("min", gulp.series("min:app", "min:resource", "min:css", "min:cssRS"), function (callback) {
    callback();
});

//Копирование/сжатие картинок и шрифтов
gulp.task("img", gulp.series("images:img", "images:moduleimg", "fonts"));

// Сначала выполняется очистка, затем сборка, затем копирование изображений и шрифтов
gulp.task("build", gulp.series("clean", "min", "img"));  //Синхронно, т.к. при асинхронном запуске min и img, иногда возникает ошибка

// отслеживание изменений
gulp.task("watch", function () {
    gulp.watch(config.appFiles, gulp.parallel("clean:app", "min:app"));
    gulp.watch(config.cssFiles, gulp.parallel("clean:css", "min:css"));
    gulp.watch(config.cssRSFiles, gulp.parallel("clean:cssRS", "min:cssRS"));
    gulp.watch(config.resourceFiles, gulp.parallel("clean:res", "min:resource"));
});
// Запускается автоматически при открытии проекта
gulp.task('default', gulp.series('watch'));