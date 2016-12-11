"use strict";
var Commands_1 = require('../../Messages/Commands');
var CreateAuctionCtrl = (function () {
    //TODO: inject interface
    function CreateAuctionCtrl(createAuctionCommandHandler) {
        this.createAuctionCommandHandler = createAuctionCommandHandler;
        this.model = new Commands_1.CreateAuctionCommand();
        this.fields = [
            {
                key: 'title',
                type: 'input',
                templateOptions: {
                    label: 'Title'
                }
            },
            {
                key: 'description',
                type: 'textarea',
                templateOptions: {
                    label: 'Description'
                }
            }
        ];
    }
    CreateAuctionCtrl.prototype.submit = function () {
        this.createAuctionCommandHandler.handle(this.model);
    };
    CreateAuctionCtrl.$inject = ['CreateAuctionCommandHandler'];
    return CreateAuctionCtrl;
}());
exports.CreateAuctionCtrl = CreateAuctionCtrl;
//# sourceMappingURL=CreateAuctionCtrl.js.map