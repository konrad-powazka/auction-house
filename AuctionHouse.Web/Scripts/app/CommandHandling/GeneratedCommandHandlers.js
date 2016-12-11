"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var CommandHandler_1 = require('./CommandHandler');
var CancelAuctionCommandHandler = (function (_super) {
    __extends(CancelAuctionCommandHandler, _super);
    function CancelAuctionCommandHandler() {
        _super.apply(this, arguments);
    }
    CancelAuctionCommandHandler.prototype.getCommandName = function () {
        return 'CancelAuctionCommand';
    };
    return CancelAuctionCommandHandler;
}(CommandHandler_1.CommandHandler));
exports.CancelAuctionCommandHandler = CancelAuctionCommandHandler;
var CreateAuctionCommandHandler = (function (_super) {
    __extends(CreateAuctionCommandHandler, _super);
    function CreateAuctionCommandHandler() {
        _super.apply(this, arguments);
    }
    CreateAuctionCommandHandler.prototype.getCommandName = function () {
        return 'CreateAuctionCommand';
    };
    return CreateAuctionCommandHandler;
}(CommandHandler_1.CommandHandler));
exports.CreateAuctionCommandHandler = CreateAuctionCommandHandler;
var MakeBidCommandHandler = (function (_super) {
    __extends(MakeBidCommandHandler, _super);
    function MakeBidCommandHandler() {
        _super.apply(this, arguments);
    }
    MakeBidCommandHandler.prototype.getCommandName = function () {
        return 'MakeBidCommand';
    };
    return MakeBidCommandHandler;
}(CommandHandler_1.CommandHandler));
exports.MakeBidCommandHandler = MakeBidCommandHandler;
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
exports.AngularCommandHandlersRegistry = AngularCommandHandlersRegistry;
//# sourceMappingURL=GeneratedCommandHandlers.js.map