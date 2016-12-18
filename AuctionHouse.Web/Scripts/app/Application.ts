import { INamedComponentOptions } from './Infrastructure/INamedComponentOptions';
import { CreateAuctionComponent } from './UI/Auctions/CreateAuctionComponent';
import { AngularCommandHandlersRegistry } from './CommandHandling/GeneratedCommandHandlers';

export class Application {
    private static components: INamedComponentOptions[] = [
        new CreateAuctionComponent()
    ];

    static bootstrap(): void {
        const module = angular.module('auctionHouse',
        ['ui.router', 'formly', 'formlyBootstrap', 'ngMessages', 'ngAnimate'] as string[]);

        module.service(AngularCommandHandlersRegistry.commandHandlers);

        for (let component of Application.components) {
            module.component(component.registerAs, component);
        }

        Application.configureModule.$inject = ['$stateProvider'];
        module.config(Application.configureModule);
        Application.runModule.$inject = ['formlyConfig', 'formlyValidationMessages'];
        module.run(Application.runModule);
    };

    private static configureModule($stateProvider: ng.ui.IStateProvider): void {
        Application.configureRouting($stateProvider);
    }

    private static runModule(formlyConfig: AngularFormly.IFormlyConfig,
        formlyValidationMessages: AngularFormly.IValidationMessages): void {
        Application.configureFormly(formlyConfig, formlyValidationMessages);
    }

    private static configureRouting($stateProvider: ng.ui.IStateProvider): void {
        const states: ng.ui.IState[] = [
            {
                name: 'createAuction',
                url: '/auction/create',
                component: 'createAuction'
            }
        ];

        for (let state of states) {
            $stateProvider.state(state);
        }
    };

    // Reference at http://angular-formly.com/#/example/other/error-summary
    private static configureFormly(formlyConfig: AngularFormly.IFormlyConfig,
        formlyValidationMessages: AngularFormly.IValidationMessages): void {

        formlyConfig.setWrapper({
            name: 'validation',
            types: ['input', 'textarea'],
            templateUrl: 'Template/Shared/AngularFormlyErrorMessagesInputWrapper'
        });

        formlyValidationMessages
            .addTemplateOptionValueMessage('maxlength', 'maxlength', '', 'is the maximum length', 'Too long');

        formlyValidationMessages
            .addTemplateOptionValueMessage('minlength', 'minlength', '', 'is the minimum length', 'Too short');

        formlyValidationMessages
            .addTemplateOptionValueMessage('required', 'label', '', 'is required', 'This field is required');
    };
}

Application.bootstrap();