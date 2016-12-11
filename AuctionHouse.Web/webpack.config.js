module.exports = {
    entry: './Scripts/app/Application.ts',
    output: {
        filename: 'Scripts/app/ApplicationBundle.js'
    },
    resolve: {
        extensions: ['', '.Webpack.js', '.web.js', '.ts', '.js', '.tsx']
    },
    module: {
        loaders: [
            {
                test: /\.tsx?$/,
                exclude: /(node_modules|bower_components)/,
                loader: 'ts-loader'
            }
        ]
    }
}