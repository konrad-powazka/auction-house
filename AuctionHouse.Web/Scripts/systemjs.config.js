/**
 * System configuration for Angular 2 samples
 * Adjust as necessary for your application needs.
 */
(function (global) {
    // map tells the System loader where to look for things
    var map = {
        'app': 'scripts/app', // 'dist',
        '@angular': 'dependencies/scripts/angular',
        'angular2-in-memory-web-api': 'dependencies/scripts/angular',
    };
    // packages tells the System loader how to load when no filename and/or no extension
    var packages = {
        'app': { main: 'main.js', defaultExtension: 'js' },
        'angular2-in-memory-web-api': { main: 'index.js', defaultExtension: 'js' }
    };
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

    ngPackageNames.forEach(function packUmd(pkgName) {

        packages['@angular/' + pkgName] = {
            main: pkgName + '.umd.js',
            defaultExtension: 'js'
        };
    });

    var bundles = {
        'dependencies/scripts/Rx.umd.js': ['rxjs/*']
    };

    var config = {
        //bundles: bundles,
        map: map,
        packages: packages,
        paths: {
            'rxjs/*': 'dependencies/scripts/Rx.umd.js'
        }
    };
    System.config(config);
})(this);
