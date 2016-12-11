"use strict";
var CommandHandler = (function () {
    function CommandHandler(httpService) {
        this.httpService = httpService;
    }
    CommandHandler.prototype.handle = function (command) {
        var url = "api/" + this.getCommandName() + "/Handle";
        return this.httpService.post(url, command).then(function () { });
    };
    CommandHandler.$inject = ['$http'];
    return CommandHandler;
}());
exports.CommandHandler = CommandHandler;
//# sourceMappingURL=CommandHandler.js.map