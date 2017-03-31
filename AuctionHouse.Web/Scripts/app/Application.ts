import { INamedComponentOptions } from './Infrastructure/INamedComponentOptions';
import { CreateAuctionComponent } from './UI/Auctions/CreateAuctionComponent';
import { AngularCommandHandlersRegistry } from './CommandHandling/GeneratedCommandHandlers';
import { SecurityService } from './Security/SecurityService';
import { SecurityUiService } from './UI/Shared/SecurityUiService';
import { SignInDialogComponent } from './UI/Shared/SignInDialogComponent';
import { ApplicationCtrl } from './UI/Shared/ApplicationCtrl';
import { Routing } from './Routing';
import { AngularQueryHandlersRegistry } from './QueryHandling/GeneratedQueryHandlers';
import { DisplayAuctionComponent } from './UI/Auctions/DisplayAuctionComponent';
import BusyIndicator from './UI/Shared/BusyIndicator';
import { AngularCommandUiHandlersRegistry } from './UI/Shared/CommandHandling/GeneratedUiCommandHandlers';
import { AuctionsListComponent } from './UI/Auctions/AuctionsListComponent';
import BusyIndicatingHttpInterceptor from './UI/Shared/BusyIndicatingHttpInterceptor';
import { SimpleNotificationDialogComponent } from './UI/Shared/SimpleNotificationDialogComponent';
import GenericModalService from './UI/Shared/GenericModalService';
import Configuration from './Configuration';
import FormatDateTimeFilterFactory from './UI/Shared/FormatDateTimeFilterFactory';
import { ComposeUserMessageDialogComponent } from './UI/UserMessaging/ComposeUserMessageDialogComponent';
import { UserReferenceComponent } from './UI/Shared/UserReferenceComponent';
import { UserMessagesComponent } from './UI/UserMessaging/UserMessagesComponent';
import { UserAuctionsListComponent } from './UI/Auctions/UserAuctionsListComponent';
import { ActiveAuctionsListComponent } from './UI/Auctions/ActiveAuctionsListComponent';
import { UserMessagesListComponent } from './UI/UserMessaging/UserMessagesListComponent';
import { NewLinesToParagraphsComponent } from './UI/Shared/NewLinesToParagraphsComponent';
import {DisplayUserMessageDialogComponent} from './UI/UserMessaging/DisplayUserMessageDialogComponent';

export class Application {
	private static components: INamedComponentOptions[] = [
		new AuctionsListComponent(),
		new CreateAuctionComponent(),
		new DisplayAuctionComponent(),
		new SignInDialogComponent(),
		new SimpleNotificationDialogComponent(),
		new UserReferenceComponent(),
		new ComposeUserMessageDialogComponent(),
		new UserMessagesComponent(),
		new UserAuctionsListComponent(),
		new ActiveAuctionsListComponent(),
		new UserMessagesListComponent(),
		new NewLinesToParagraphsComponent(),
		new DisplayUserMessageDialogComponent()
	];

	static bootstrap(): void {
		const module = angular.module('auctionHouse',
			[
				'ui.router', 'formly', 'formlyBootstrap', 'ngMessages', 'ngAnimate', 'ui.bootstrap',
				'ui.bootstrap.datetimepicker', 'angularSpinner', 'ngTasty'
			]);

		this.registerConstants(module);

		Application.configureModule.$inject = [
			'$stateProvider', '$urlRouterProvider', '$httpProvider'
		];

		module.config(Application.configureModule);
		this.registerSerivces(module);
		module.controller('applicationCtrl', ApplicationCtrl);

		for (let component of Application.components) {
			module.component(component.registerAs, component);
		}

		Application.runModule.$inject = [
			'formlyConfig', 'formlyValidationMessages'
		];

		module.run(Application.runModule);
	};

	private static registerSerivces(module: ng.IModule): void {
		module.service(AngularCommandHandlersRegistry.commandHandlers);
		module.service(AngularQueryHandlersRegistry.queryHandlers);
		module.service(AngularCommandUiHandlersRegistry.commandUiHandlers);
		module.service('securityService', SecurityService);
		module.service('securityUiService', SecurityUiService);
		module.service('busyIndicatingHttpInterceptor', BusyIndicatingHttpInterceptor);
		module.service('genericModalService', GenericModalService);
		module.service('configuration', Configuration);
		module.filter('formatDateTime', FormatDateTimeFilterFactory.createStandardFilterFunction);
		module.filter('formatToNowDateTime', FormatDateTimeFilterFactory.createToNowFilterFunction);
	}

	private static registerConstants(module: ng.IModule): void {
		module.constant('busyIndicator', new BusyIndicator());
	}

	private static configureModule($stateProvider: ng.ui.IStateProvider,
		$urlRouterProvider: ng.ui.IUrlRouterProvider,
		$httpProvider: ng.IHttpProvider): void {
		Routing.configure($stateProvider, $urlRouterProvider);
		$httpProvider.interceptors.push([
			'busyIndicatingHttpInterceptor',
			(busyIndicatingHttpInterceptor: BusyIndicatingHttpInterceptor) => busyIndicatingHttpInterceptor
		]);
	}

	private static runModule(formlyConfig: AngularFormly.IFormlyConfig,
		formlyValidationMessages: AngularFormly.IValidationMessages): void {
		Application.configureFormly(formlyConfig, formlyValidationMessages);
	}

	// Reference at http://angular-formly.com/#/example/other/error-summary
	private static configureFormly(formlyConfig: AngularFormly.IFormlyConfig,
		formlyValidationMessages: AngularFormly.IValidationMessages): void {

		formlyConfig.setType({
			name: 'dateTimePicker',
			template:
			'<div><datetimepicker ng-model="model[options.key]" show-spinners="true" date-format="M/d/yyyy" date-options="dateOptions"></datetimepicker></div>',
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

		formlyValidationMessages
			.addTemplateOptionValueMessage('min', 'min', 'Minimal value is', '', 'Too small');

		formlyConfig.extras.errorExistsAndShouldBeVisibleExpression = 'fc.$touched || form.$submitted';
	};
}

Application.bootstrap();