namespace AuctionHouse {
    import Injectable = angular.Injectable;
    import IControllerConstructor = angular.IControllerConstructor;
    import CreateAuctionCtrl = Auctions.CreateAuctionCtrl;
    import ViewAuctionCtrl = Auctions.ViewAuctionCtrl;

    export class Application {
        private static controllers: { [controllerName: string]: Injectable<IControllerConstructor>; } = {
            'CreateAuctionCtrl': CreateAuctionCtrl,
            'ViewAuctionCtrl': ViewAuctionCtrl
        };

        static bootstrap(): void {
            const module = angular.module('auctionHouse', ['formly', 'formlyBootstrap'] as string[]);

            for (let controllerName in Application.controllers) {
                if (Application.controllers.hasOwnProperty(controllerName)) {
                    const controllerCtor = Application.controllers[controllerName];
                    module.controller(controllerName, controllerCtor);
                }
            }
        };
    }

    Application.bootstrap();
}