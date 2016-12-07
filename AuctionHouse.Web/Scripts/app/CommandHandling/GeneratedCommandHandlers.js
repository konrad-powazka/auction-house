var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var AuctionHouse;
(function (AuctionHouse) {
    var CommandHandling;
    (function (CommandHandling) {
        var CancelAuctionCommandHandler = (function (_super) {
            __extends(CancelAuctionCommandHandler, _super);
            function CancelAuctionCommandHandler() {
                _super.apply(this, arguments);
            }
            CancelAuctionCommandHandler.prototype.getCommandName = function () {
                return 'CancelAuctionCommand';
            };
            return CancelAuctionCommandHandler;
        }(CommandHandling.CommandHandler));
        CommandHandling.CancelAuctionCommandHandler = CancelAuctionCommandHandler;
        var CreateAuctionCommandHandler = (function (_super) {
            __extends(CreateAuctionCommandHandler, _super);
            function CreateAuctionCommandHandler() {
                _super.apply(this, arguments);
            }
            CreateAuctionCommandHandler.prototype.getCommandName = function () {
                return 'CreateAuctionCommand';
            };
            return CreateAuctionCommandHandler;
        }(CommandHandling.CommandHandler));
        CommandHandling.CreateAuctionCommandHandler = CreateAuctionCommandHandler;
        var MakeBidCommandHandler = (function (_super) {
            __extends(MakeBidCommandHandler, _super);
            function MakeBidCommandHandler() {
                _super.apply(this, arguments);
            }
            MakeBidCommandHandler.prototype.getCommandName = function () {
                return 'MakeBidCommand';
            };
            return MakeBidCommandHandler;
        }(CommandHandling.CommandHandler));
        CommandHandling.MakeBidCommandHandler = MakeBidCommandHandler;
        var AngularCommandHandlersRegistry = (function () {
            function AngularCommandHandlersRegistry() {
            }
            AngularCommandHandlersRegistry.commandHandlers = {
                'CancelAuctionCommandHandler': CancelAuctionCommandHandler,
                'CreateAuctionCommandHandler': CreateAuctionCommandHandler,
                'MakeBidCommandHandler': MakeBidCommandHandler,
            };
            return AngularCommandHandlersRegistry;
        }());
        CommandHandling.AngularCommandHandlersRegistry = AngularCommandHandlersRegistry;
    })(CommandHandling = AuctionHouse.CommandHandling || (AuctionHouse.CommandHandling = {}));
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=GeneratedCommandHandlers.js.map