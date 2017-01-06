import { INamedComponentOptions } from './Infrastructure/INamedComponentOptions';
import { CreateAuctionComponent } from './UI/Auctions/CreateAuctionComponent';
import { AngularCommandHandlersRegistry } from './CommandHandling/GeneratedCommandHandlers';
import {SecurityService} from './Security/SecurityService';
import {SecurityUiService} from './UI/Shared/SecurityUiService';
import {LoginDialogComponent} from './UI/Shared/LoginDialogComponent';
import {ApplicationCtrl} from './UI/Shared/ApplicationCtrl';

export class Application {
    private static components: INamedComponentOptions[] = [
        new CreateAuctionComponent(),
        new LoginDialogComponent()
    ];

    static bootstrap(): void {
        const module = angular.module('auctionHouse',
        [
            'ui.router', 'formly', 'formlyBootstrap', 'ngMessages', 'ngAnimate', 'ui.bootstrap',
            'ui.bootstrap.datetimepicker'
        ]);

        module.controller('applicationCtrl', ApplicationCtrl);

        this.registerSerivces(module);

        for (let component of Application.components) {
            module.component(component.registerAs, component);
        }

        Application.configureModule.$inject = ['$stateProvider'];
        module.config(Application.configureModule);
        Application.runModule.$inject = ['formlyConfig', 'formlyValidationMessages'];
        module.run(Application.runModule);
    };

    private static registerSerivces(module: ng.IModule): void {
        module.service(AngularCommandHandlersRegistry.commandHandlers);
        module.service('securityService', SecurityService);
        module.service('securityUiService', SecurityUiService);
    }

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

        formlyConfig.setType({
            name: 'dateTimePicker',
            template: '<div><datetimepicker ng-model="model[options.key]" show-spinners="true" date-format="M/d/yyyy" date-options="dateOptions"></datetimepicker></div>',
            wrapper: ['bootstrapLabel', 'bootstrapHasError'],
            defaultOptions: {
                templateOptions: {
                    label: 'Time'
                }
            }
        });

        formlyConfig.setWrapper({
            name: 'validation',
            types: ['input', 'textarea', 'dateTimePicker'],
            templateUrl: 'Template/Shared/AngularFormlyErrorMessagesInputWrapper'
        });

        formlyValidationMessages
            .addTemplateOptionValueMessage('maxlength', 'maxlength', '', 'is the maximum length', 'Too long');

        formlyValidationMessages
            .addTemplateOptionValueMessage('minlength', 'minlength', '', 'is the minimum length', 'Too short');

        formlyValidationMessages
            .addTemplateOptionValueMessage('required', 'label', '', 'is required', 'This field is required');

        formlyConfig.extras.errorExistsAndShouldBeVisibleExpression = 'fc.$touched || form.$submitted';
    };
}

Application.bootstrap();