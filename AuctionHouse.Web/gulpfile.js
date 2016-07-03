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
            dependenciesSourceRoot + 'zone.js/dist/zone.js',
            dependenciesSourceRoot + 'reflect-metadata/Reflect.js',
            dependenciesSourceRoot + 'systemjs/dist/system.src.js',
            dependenciesSourceRoot + 'rxjs/bundles/Rx.umd.js',
            dependenciesSourceRoot + 'ng2-bootstrap/bundles/ng2-bootstrap.js'
        ])
        .pipe(gulp.dest(scriptDependenciesDestinationRoot));

    var angularScriptDependenciesDestinationRoot = scriptDependenciesDestinationRoot + 'angular/';

    var ngPackageNames = [
      'common',
      'compiler',
      'core',
      'forms',
      'http',
      'platform-browser',
      'platform-browser-dynamic',
      'router',
      'router-deprecated',
      'upgrade'
    ];

    for (var i = 0; i < ngPackageNames.length; i++) {
        var ngPackageName = ngPackageNames[i];
        var ngPackagePath = dependenciesSourceRoot + '@angular/' + ngPackageName + '/bundles/' + ngPackageName + '.umd.js';
        var ngPackageDestPath = angularScriptDependenciesDestinationRoot + ngPackageName;

        gulp.src(ngPackagePath)
        .pipe(gulp.dest(ngPackageDestPath));
    }

    gulp.src([
        "node_modules/bootstrap/dist/css/bootstrap.css"
    ]).pipe(gulp.dest(dependenciesDestinationRoot + 'styles/'));

    gulp.src([
            './Scripts/**/*.js'
        ], { base: './Scripts/' })
        .pipe(gulp.dest(destinationRoot + 'scripts/'));
});