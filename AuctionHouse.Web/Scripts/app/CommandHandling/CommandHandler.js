var AuctionHouse;
(function (AuctionHouse) {
    var CommandHandling;
    (function (CommandHandling) {
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
        CommandHandling.CommandHandler = CommandHandler;
    })(CommandHandling = AuctionHouse.CommandHandling || (AuctionHouse.CommandHandling = {}));
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=CommandHandler.js.map