/// <binding BeforeBuild='default' Clean='clean' />

"use strict";

var gulp = require('gulp');
var del = require('del');

var destinationRoot = './wwwroot/';
var dependenciesSourceRoot = './node_modules/';


gulp.task('clean', function () {
    del.sync([destinationRoot + '/**']);
});

gulp.task('default', ['clean'], function () {
    var dependenciesDestinationRoot = destinationRoot + 'dependencies/';
    var scriptDependenciesDestinationRoot = dependenciesDestinationRoot + 'scripts/';

    gulp.src([
        dependenciesSourceRoot + 'rxjs/bundles/Rx.umd.js'
    ])
    .pipe(gulp.dest(scriptDependenciesDestinationRoot));

    var angularScriptDependenciesDestinationRoot = scriptDependenciesDestinationRoot + 'angular/';

    gulp.src([
            dependenciesSourceRoot + '@angular/core/bundles/core.umd.js',
            dependenciesSourceRoot + '@angular/common/bundles/common.umd.js',
            dependenciesSourceRoot + '@angular/compiler/bundles/compiler.umd.js',
            dependenciesSourceRoot + '@angular/platform-browser/bundles/platform-browser.umd.js',
            dependenciesSourceRoot + '@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js'
            //dependenciesSourceRoot + 'bootstrap/dist/js/bootstrap.js',
            //dependenciesSourceRoot + 'systemjs/dist/system.js',
            //dependenciesSourceRoot + 'typescript/lib/typescript.js',
        ])
        .pipe(gulp.dest(angularScriptDependenciesDestinationRoot));

    gulp.src([
        "node_modules/bootstrap/dist/css/bootstrap.css"
    ]).pipe(gulp.dest(dependenciesDestinationRoot + 'styles/'));

    gulp.src([
            './Scripts/**/*.js'
        ], { base: './Scripts/' })
        .pipe(gulp.dest(destinationRoot + 'scripts/'));
});