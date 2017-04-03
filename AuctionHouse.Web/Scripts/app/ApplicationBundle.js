/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId])
/******/ 			return installedModules[moduleId].exports;
/******/
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			exports: {},
/******/ 			id: moduleId,
/******/ 			loaded: false
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.loaded = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var CreateAuctionComponent_1 = __webpack_require__(1);
	var GeneratedCommandHandlers_1 = __webpack_require__(5);
	var SecurityService_1 = __webpack_require__(8);
	var SecurityUiService_1 = __webpack_require__(9);
	var SignInDialogComponent_1 = __webpack_require__(10);
	var ApplicationCtrl_1 = __webpack_require__(12);
	var Routing_1 = __webpack_require__(13);
	var GeneratedQueryHandlers_1 = __webpack_require__(14);
	var DisplayAuctionComponent_1 = __webpack_require__(16);
	var BusyIndicator_1 = __webpack_require__(18);
	var GeneratedUiCommandHandlers_1 = __webpack_require__(19);
	var AuctionsListComponent_1 = __webpack_require__(22);
	var BusyIndicatingHttpInterceptor_1 = __webpack_require__(26);
	var SimpleNotificationDialogComponent_1 = __webpack_require__(27);
	var GenericModalService_1 = __webpack_require__(29);
	var Configuration_1 = __webpack_require__(30);
	var FormatDateTimeFilterFactory_1 = __webpack_require__(31);
	var ComposeUserMessageDialogComponent_1 = __webpack_require__(32);
	var UserReferenceComponent_1 = __webpack_require__(34);
	var UserMessagesComponent_1 = __webpack_require__(36);
	var UserAuctionsListComponent_1 = __webpack_require__(38);
	var ActiveAuctionsListComponent_1 = __webpack_require__(40);
	var UserMessagesListComponent_1 = __webpack_require__(42);
	var NewLinesToParagraphsComponent_1 = __webpack_require__(44);
	var DisplayUserMessageDialogComponent_1 = __webpack_require__(46);
	var Application = (function () {
	    function Application() {
	    }
	    Application.bootstrap = function () {
	        var module = angular.module('auctionHouse', [
	            'ui.router', 'formly', 'formlyBootstrap', 'ngMessages', 'ngAnimate', 'ui.bootstrap',
	            'ui.bootstrap.datetimepicker', 'angularSpinner', 'ngTasty'
	        ]);
	        this.registerConstants(module);
	        Application.configureModule.$inject = [
	            '$stateProvider', '$urlRouterProvider', '$httpProvider'
	        ];
	        module.config(Application.configureModule);
	        this.registerSerivces(module);
	        module.controller('applicationCtrl', ApplicationCtrl_1.ApplicationCtrl);
	        for (var _i = 0, _a = Application.components; _i < _a.length; _i++) {
	            var component = _a[_i];
	            module.component(component.registerAs, component);
	        }
	        Application.runModule.$inject = [
	            'formlyConfig', 'formlyValidationMessages'
	        ];
	        module.run(Application.runModule);
	    };
	    ;
	    Application.registerSerivces = function (module) {
	        module.service(GeneratedCommandHandlers_1.AngularCommandHandlersRegistry.commandHandlers);
	        module.service(GeneratedQueryHandlers_1.AngularQueryHandlersRegistry.queryHandlers);
	        module.service(GeneratedUiCommandHandlers_1.AngularCommandUiHandlersRegistry.commandUiHandlers);
	        module.service('securityService', SecurityService_1.SecurityService);
	        module.service('securityUiService', SecurityUiService_1.SecurityUiService);
	        module.service('busyIndicatingHttpInterceptor', BusyIndicatingHttpInterceptor_1.default);
	        module.service('genericModalService', GenericModalService_1.default);
	        module.service('configuration', Configuration_1.default);
	        module.filter('formatDateTime', FormatDateTimeFilterFactory_1.default.createStandardFilterFunction);
	        module.filter('formatToNowDateTime', FormatDateTimeFilterFactory_1.default.createToNowFilterFunction);
	    };
	    Application.registerConstants = function (module) {
	        module.constant('busyIndicator', new BusyIndicator_1.default());
	    };
	    Application.configureModule = function ($stateProvider, $urlRouterProvider, $httpProvider) {
	        Routing_1.Routing.configure($stateProvider, $urlRouterProvider);
	        $httpProvider.interceptors.push([
	            'busyIndicatingHttpInterceptor',
	            function (busyIndicatingHttpInterceptor) { return busyIndicatingHttpInterceptor; }
	        ]);
	    };
	    Application.runModule = function (formlyConfig, formlyValidationMessages) {
	        Application.configureFormly(formlyConfig, formlyValidationMessages);
	    };
	    // Reference at http://angular-formly.com/#/example/other/error-summary
	    Application.configureFormly = function (formlyConfig, formlyValidationMessages) {
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
	        formlyValidationMessages
	            .addTemplateOptionValueMessage('min', 'min', 'Minimal value is', '', 'Too small');
	        formlyConfig.extras.errorExistsAndShouldBeVisibleExpression = 'fc.$touched || form.$submitted';
	    };
	    ;
	    return Application;
	}());
	Application.components = [
	    new AuctionsListComponent_1.AuctionsListComponent(),
	    new CreateAuctionComponent_1.CreateAuctionComponent(),
	    new DisplayAuctionComponent_1.DisplayAuctionComponent(),
	    new SignInDialogComponent_1.SignInDialogComponent(),
	    new SimpleNotificationDialogComponent_1.SimpleNotificationDialogComponent(),
	    new UserReferenceComponent_1.UserReferenceComponent(),
	    new ComposeUserMessageDialogComponent_1.ComposeUserMessageDialogComponent(),
	    new UserMessagesComponent_1.UserMessagesComponent(),
	    new UserAuctionsListComponent_1.UserAuctionsListComponent(),
	    new ActiveAuctionsListComponent_1.ActiveAuctionsListComponent(),
	    new UserMessagesListComponent_1.UserMessagesListComponent(),
	    new NewLinesToParagraphsComponent_1.NewLinesToParagraphsComponent(),
	    new DisplayUserMessageDialogComponent_1.DisplayUserMessageDialogComponent()
	];
	exports.Application = Application;
	Application.bootstrap();


/***/ },
/* 1 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var CreateAuctionCtrl_1 = __webpack_require__(2);
	var CreateAuctionComponent = (function () {
	    function CreateAuctionComponent() {
	        this.controller = CreateAuctionCtrl_1.CreateAuctionCtrl;
	        this.templateUrl = 'Template/Auctions/Create';
	        this.registerAs = 'createAuction';
	    }
	    return CreateAuctionComponent;
	}());
	exports.CreateAuctionComponent = CreateAuctionComponent;


/***/ },
/* 2 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var GuidGenerator_1 = __webpack_require__(3);
	var CommandHandlingAsynchronityLevel_1 = __webpack_require__(4);
	var CreateAuctionCtrl = (function () {
	    function CreateAuctionCtrl(createAuctionCommandUiHandler, getAuctionDetailsQueryHandler, stateService) {
	        var _this = this;
	        this.createAuctionCommandUiHandler = createAuctionCommandUiHandler;
	        this.getAuctionDetailsQueryHandler = getAuctionDetailsQueryHandler;
	        this.stateService = stateService;
	        this.createAuctionCommandId = GuidGenerator_1.default.generateGuid();
	        this.model = {
	            id: GuidGenerator_1.default.generateGuid(),
	            title: '',
	            description: '',
	            startingPrice: 0,
	            buyNowPrice: null,
	            endDate: undefined
	        };
	        this.fields = [
	            {
	                key: 'title',
	                type: 'input',
	                templateOptions: {
	                    label: 'Title',
	                    required: true,
	                    minlength: 5,
	                    maxlength: 200
	                }
	            },
	            {
	                key: 'description',
	                type: 'textarea',
	                templateOptions: {
	                    label: 'Description',
	                    required: true,
	                    minlength: 10,
	                    maxlength: 10000,
	                    rows: 8
	                }
	            },
	            {
	                key: 'startingPrice',
	                type: 'input',
	                templateOptions: {
	                    label: 'Starting price',
	                    required: true,
	                    type: 'number',
	                    min: 0,
	                    onChange: function () {
	                        _this.form['buyNowPrice'].$validate();
	                    }
	                }
	            },
	            {
	                key: 'buyNowPrice',
	                name: 'buyNowPrice',
	                type: 'input',
	                templateOptions: {
	                    label: 'Buy now price',
	                    required: false,
	                    type: 'number'
	                },
	                validators: {
	                    greaterOrEqualToStartingPrice: {
	                        expression: function ($viewValue, $modelValue) {
	                            var value = $modelValue || $viewValue;
	                            return !_(value).isNumber() || !_(_this.model.startingPrice).isNumber() || value >= _this.model.startingPrice;
	                        },
	                        message: function () { return 'Buy now price cannot be smaller than starting price'; }
	                    }
	                }
	            },
	            {
	                key: 'endDate',
	                type: 'dateTimePicker',
	                templateOptions: {
	                    label: 'End date and time',
	                    required: true
	                },
	                validators: {
	                    futureDate: {
	                        expression: function ($viewValue, $modelValue) {
	                            var rawValue = $modelValue || $viewValue;
	                            var momentValue = moment(rawValue);
	                            return momentValue.isSameOrAfter(moment().add(5, 'minutes'));
	                        },
	                        message: function ($viewValue, $modelValue) {
	                            var rawValue = $modelValue || $viewValue;
	                            return 'Please select a date at least 5 minutes in the future';
	                        }
	                    }
	                }
	            }
	        ];
	    }
	    CreateAuctionCtrl.prototype.submit = function () {
	        var _this = this;
	        if (!this.form.$valid) {
	            return;
	        }
	        this.createAuctionCommandUiHandler
	            .handle(this.model, this.createAuctionCommandId, CommandHandlingAsynchronityLevel_1.CommandHandlingAsynchronityLevel.WaitUnitReadModelIsUpdated)
	            .then(function () {
	            _this.stateService.go('displayAuction', { auctionId: _this.model.id });
	        });
	    };
	    return CreateAuctionCtrl;
	}());
	CreateAuctionCtrl.$inject = ['createAuctionCommandUiHandler', 'getAuctionDetailsQueryHandler', '$state'];
	exports.CreateAuctionCtrl = CreateAuctionCtrl;


/***/ },
/* 3 */
/***/ function(module, exports) {

	"use strict";
	var GuidGenerator = (function () {
	    function GuidGenerator() {
	    }
	    GuidGenerator.generateGuid = function () {
	        function s4() {
	            return Math.floor((1 + Math.random()) * 0x10000)
	                .toString(16)
	                .substring(1);
	        }
	        return s4() +
	            s4() +
	            '-' +
	            s4() +
	            '-' +
	            s4() +
	            '-' +
	            s4() +
	            '-' +
	            s4() +
	            s4() +
	            s4();
	    };
	    return GuidGenerator;
	}());
	Object.defineProperty(exports, "__esModule", { value: true });
	exports.default = GuidGenerator;


/***/ },
/* 4 */
/***/ function(module, exports) {

	"use strict";
	var CommandHandlingAsynchronityLevel;
	(function (CommandHandlingAsynchronityLevel) {
	    // TODO: Add QueueOnly
	    CommandHandlingAsynchronityLevel[CommandHandlingAsynchronityLevel["WaitUntilCommandIsProcessed"] = 0] = "WaitUntilCommandIsProcessed";
	    CommandHandlingAsynchronityLevel[CommandHandlingAsynchronityLevel["WaitUnitReadModelIsUpdated"] = 1] = "WaitUnitReadModelIsUpdated";
	})(CommandHandlingAsynchronityLevel = exports.CommandHandlingAsynchronityLevel || (exports.CommandHandlingAsynchronityLevel = {}));


/***/ },
/* 5 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var CommandHandler_1 = __webpack_require__(6);
	var SendUserMessageCommandHandler = (function (_super) {
	    __extends(SendUserMessageCommandHandler, _super);
	    function SendUserMessageCommandHandler() {
	        return _super.apply(this, arguments) || this;
	    }
	    SendUserMessageCommandHandler.prototype.getCommandName = function () {
	        return 'SendUserMessageCommand';
	    };
	    return SendUserMessageCommandHandler;
	}(CommandHandler_1.CommandHandler));
	exports.SendUserMessageCommandHandler = SendUserMessageCommandHandler;
	var CreateAuctionCommandHandler = (function (_super) {
	    __extends(CreateAuctionCommandHandler, _super);
	    function CreateAuctionCommandHandler() {
	        return _super.apply(this, arguments) || this;
	    }
	    CreateAuctionCommandHandler.prototype.getCommandName = function () {
	        return 'CreateAuctionCommand';
	    };
	    return CreateAuctionCommandHandler;
	}(CommandHandler_1.CommandHandler));
	exports.CreateAuctionCommandHandler = CreateAuctionCommandHandler;
	var FinishAuctionCommandHandler = (function (_super) {
	    __extends(FinishAuctionCommandHandler, _super);
	    function FinishAuctionCommandHandler() {
	        return _super.apply(this, arguments) || this;
	    }
	    FinishAuctionCommandHandler.prototype.getCommandName = function () {
	        return 'FinishAuctionCommand';
	    };
	    return FinishAuctionCommandHandler;
	}(CommandHandler_1.CommandHandler));
	exports.FinishAuctionCommandHandler = FinishAuctionCommandHandler;
	var MakeBidCommandHandler = (function (_super) {
	    __extends(MakeBidCommandHandler, _super);
	    function MakeBidCommandHandler() {
	        return _super.apply(this, arguments) || this;
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
	    return AngularCommandHandlersRegistry;
	}());
	AngularCommandHandlersRegistry.commandHandlers = {
	    'sendUserMessageCommandHandler': SendUserMessageCommandHandler,
	    'createAuctionCommandHandler': CreateAuctionCommandHandler,
	    'finishAuctionCommandHandler': FinishAuctionCommandHandler,
	    'makeBidCommandHandler': MakeBidCommandHandler,
	};
	exports.AngularCommandHandlersRegistry = AngularCommandHandlersRegistry;


/***/ },
/* 6 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var CommandHandlingErrorType_1 = __webpack_require__(7);
	var CommandHandlingAsynchronityLevel_1 = __webpack_require__(4);
	var CommandHandler = (function () {
	    function CommandHandler(httpService, qService, timeoutService, configuration) {
	        this.httpService = httpService;
	        this.qService = qService;
	        this.timeoutService = timeoutService;
	        this.configuration = configuration;
	        if (!CommandHandler.wasSignalrRInitialized) {
	            var connection = $.connection;
	            var commandHandlingFeedbackHub = connection.commandHandlingFeedbackHub;
	            commandHandlingFeedbackHub.client.handleCommandSuccess = function (commandHandlingSucceededEvent) {
	                CommandHandler.commandHandlingSuccessCallbacks.fire(commandHandlingSucceededEvent);
	            };
	            commandHandlingFeedbackHub.client.handleCommandFailure = function (commandHandlingFailedEvent) {
	                CommandHandler.commandHandlingFailureCallbacks.fire(commandHandlingFailedEvent);
	            };
	            var eventAppliedToReadModelNotificationHub = connection.eventAppliedToReadModelNotificationHub;
	            eventAppliedToReadModelNotificationHub.client
	                .handleEventsAppliedToReadModel = function (subscriptionId) {
	                CommandHandler.eventsAppliedToReadModelCallbacks.fire(subscriptionId);
	            };
	            CommandHandler
	                .eventAppliedToReadModelNotificationHubServer = eventAppliedToReadModelNotificationHub.server;
	            CommandHandler.wasSignalrRInitialized = true;
	        }
	    }
	    CommandHandler.prototype.handle = function (command, commandId, asynchronityLevel) {
	        var _this = this;
	        var deferred = this.qService.defer();
	        this.connectSignalR()
	            .then(function () {
	            _this.sendCommandAndWaitForHandling(command, commandId, asynchronityLevel, deferred);
	        })
	            .catch(function () { return deferred.reject(CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToConnectToFeedbackHub); });
	        return deferred.promise;
	    };
	    CommandHandler.prototype.sendCommandAndWaitForHandling = function (command, commandId, asynchronityLevel, deferred) {
	        var _this = this;
	        var commandProcessingFinishedAndSucceeded = false;
	        var commandHandlingSuccessCallback = function (commandHandlingSucceededEvent) {
	            if (commandHandlingSucceededEvent.commandId === commandId) {
	                commandProcessingFinishedAndSucceeded = true;
	                if (asynchronityLevel === CommandHandlingAsynchronityLevel_1.CommandHandlingAsynchronityLevel.WaitUntilCommandIsProcessed) {
	                    deferred.resolve();
	                }
	                else {
	                    _this
	                        .waitForEventsApplicationToReadModel(commandHandlingSucceededEvent
	                        .publishedEventIds, deferred);
	                }
	            }
	        };
	        var commandHandlingFailureCallback = function (commandHandlingFailedEvent) {
	            if (commandHandlingFailedEvent.commandId === commandId) {
	                deferred.reject(CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToProcess);
	            }
	        };
	        CommandHandler.commandHandlingSuccessCallbacks.add(commandHandlingSuccessCallback);
	        CommandHandler.commandHandlingFailureCallbacks.add(commandHandlingFailureCallback);
	        this.sendCommand(command, commandId)
	            .then(function () {
	            _this.timeoutService(_this.configuration.commandHandlingTimeoutMilliseconds)
	                .then(function () {
	                if (!commandProcessingFinishedAndSucceeded) {
	                    deferred.reject(CommandHandlingErrorType_1.CommandHandlingErrorType.ProcessingTimeout);
	                }
	            });
	        })
	            .catch(function () { return deferred.reject(CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToQueue); });
	        var removeCallbacks = function () {
	            CommandHandler.commandHandlingSuccessCallbacks.remove(commandHandlingSuccessCallback);
	            CommandHandler.commandHandlingFailureCallbacks.remove(commandHandlingFailureCallback);
	        };
	        deferred.promise.finally(removeCallbacks);
	    };
	    CommandHandler.prototype.sendCommand = function (command, commandId) {
	        var url = "api/" + this.getCommandName() + "/Handle?commandId=" + commandId;
	        return this.httpService.post(url, command);
	    };
	    CommandHandler.prototype.waitForEventsApplicationToReadModel = function (publishedEventIds, deferred) {
	        var _this = this;
	        CommandHandler
	            .eventAppliedToReadModelNotificationHubServer
	            .notifyOnEventsApplied(publishedEventIds)
	            .done(function (notifyOnEventsAppliedToReadModelResponse) {
	            var readModelChangeNotificationTimeoutPromise = _this
	                .timeoutService(_this.configuration.readModelChangeNotificationTimeoutMilliseconds)
	                .then(function () {
	                deferred.reject(CommandHandlingErrorType_1.CommandHandlingErrorType.ReadModelChangeNotificationTimeout);
	            });
	            if (notifyOnEventsAppliedToReadModelResponse.wereAllEventsAlreadyApplied) {
	                deferred.resolve();
	                return;
	            }
	            var eventsAppliedCallback = function (currentSubscriptionId) {
	                if (currentSubscriptionId === notifyOnEventsAppliedToReadModelResponse.subscriptionId) {
	                    deferred.resolve();
	                    _this.timeoutService.cancel(readModelChangeNotificationTimeoutPromise);
	                }
	            };
	            CommandHandler.eventsAppliedToReadModelCallbacks.add(eventsAppliedCallback);
	            deferred.promise.finally(function () { return CommandHandler.eventsAppliedToReadModelCallbacks
	                .remove(eventsAppliedCallback); });
	            deferred.promise.catch(function () {
	                // TODO: cancel subscription on timeout
	            });
	        })
	            .fail(function () { return deferred.reject(CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToSubscribeToReadModelChangeNotification); });
	    };
	    CommandHandler.prototype.connectSignalR = function () {
	        var deferred = this.qService.defer();
	        if ($.connection.hub.state === 1 /* Connected */) {
	            deferred.resolve();
	        }
	        else {
	            $.connection.hub
	                .start()
	                .done(function () { return deferred.resolve(); })
	                .fail(function () { return deferred.reject(); });
	        }
	        return deferred.promise;
	    };
	    return CommandHandler;
	}());
	CommandHandler.wasSignalrRInitialized = false;
	CommandHandler.commandHandlingSuccessCallbacks = $.Callbacks();
	CommandHandler.commandHandlingFailureCallbacks = $.Callbacks();
	CommandHandler.eventsAppliedToReadModelCallbacks = $.Callbacks();
	CommandHandler.$inject = ['$http', '$q', '$timeout', 'configuration'];
	exports.CommandHandler = CommandHandler;


/***/ },
/* 7 */
/***/ function(module, exports) {

	"use strict";
	var CommandHandlingErrorType;
	(function (CommandHandlingErrorType) {
	    CommandHandlingErrorType[CommandHandlingErrorType["FailedToConnectToFeedbackHub"] = 0] = "FailedToConnectToFeedbackHub";
	    CommandHandlingErrorType[CommandHandlingErrorType["FailedToQueue"] = 1] = "FailedToQueue";
	    CommandHandlingErrorType[CommandHandlingErrorType["ProcessingTimeout"] = 2] = "ProcessingTimeout";
	    CommandHandlingErrorType[CommandHandlingErrorType["FailedToProcess"] = 3] = "FailedToProcess";
	    CommandHandlingErrorType[CommandHandlingErrorType["FailedToSubscribeToReadModelChangeNotification"] = 4] = "FailedToSubscribeToReadModelChangeNotification";
	    CommandHandlingErrorType[CommandHandlingErrorType["ReadModelChangeNotificationTimeout"] = 5] = "ReadModelChangeNotificationTimeout";
	})(CommandHandlingErrorType = exports.CommandHandlingErrorType || (exports.CommandHandlingErrorType = {}));


/***/ },
/* 8 */
/***/ function(module, exports) {

	"use strict";
	var SecurityService = (function () {
	    function SecurityService(httpService) {
	        var _this = this;
	        this.httpService = httpService;
	        this.currentUserName = null;
	        this.httpService.get('api/Authentication/GetCurrentUser', {})
	            .then(function (response) {
	            if (!_this.currentUserName && response.data && response.data.name) {
	                _this.currentUserName = response.data.name;
	            }
	        });
	    }
	    SecurityService.prototype.signIn = function (userName, password) {
	        var _this = this;
	        var loginCommand = {
	            userName: userName,
	            password: password
	        };
	        return this.httpService.post('api/Authentication/SignIn', loginCommand)
	            .then(function () {
	            _this.currentUserName = userName;
	            // User name might have changed, so a new SignalR connection must be established
	            $.connection.hub.stop(false, true);
	        });
	    };
	    SecurityService.prototype.signOut = function () {
	        var _this = this;
	        if (!this.checkIfUserIsAuthenticated()) {
	            throw new Error('Current user is not authenticated.');
	        }
	        return this.httpService.post('api/Authentication/SignOut', {})
	            .then(function () {
	            _this.currentUserName = null;
	        });
	    };
	    SecurityService.prototype.checkIfUserIsAuthenticated = function () {
	        return this.currentUserName !== null;
	    };
	    SecurityService.prototype.getCurrentUserName = function () {
	        if (!this.checkIfUserIsAuthenticated()) {
	            throw new Error('Current user is not authenticated.');
	        }
	        return this.currentUserName;
	    };
	    return SecurityService;
	}());
	SecurityService.$inject = ['$http'];
	exports.SecurityService = SecurityService;


/***/ },
/* 9 */
/***/ function(module, exports) {

	"use strict";
	var SecurityUiService = (function () {
	    function SecurityUiService(securityService, modalService, qService) {
	        this.securityService = securityService;
	        this.modalService = modalService;
	        this.qService = qService;
	    }
	    Object.defineProperty(SecurityUiService.prototype, "currentUserName", {
	        get: function () {
	            return this.securityService.checkIfUserIsAuthenticated() ? this.securityService.getCurrentUserName() : null;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(SecurityUiService.prototype, "isUserAuthenticated", {
	        get: function () {
	            return this.securityService.checkIfUserIsAuthenticated();
	        },
	        enumerable: true,
	        configurable: true
	    });
	    SecurityUiService.prototype.ensureUserIsAuthenticated = function () {
	        if (this.securityService.checkIfUserIsAuthenticated()) {
	            return this.qService.resolve();
	        }
	        return this.showSignInDialog();
	    };
	    SecurityUiService.prototype.showSignInDialog = function () {
	        var modalInstance = this.modalService.open({
	            component: 'signInDialog'
	        });
	        return modalInstance.result;
	    };
	    SecurityUiService.prototype.signOut = function () {
	        return this.securityService.signOut();
	    };
	    return SecurityUiService;
	}());
	SecurityUiService.$inject = ['securityService', '$uibModal', '$q'];
	exports.SecurityUiService = SecurityUiService;


/***/ },
/* 10 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var SignInDialogCtrl_1 = __webpack_require__(11);
	var SignInDialogComponent = (function () {
	    function SignInDialogComponent() {
	        this.controller = SignInDialogCtrl_1.SignInDialogCtrl;
	        this.templateUrl = 'Template/Security/SignInDialog';
	        this.registerAs = 'signInDialog';
	        this.bindings = {
	            modalInstance: '<'
	        };
	    }
	    return SignInDialogComponent;
	}());
	exports.SignInDialogComponent = SignInDialogComponent;


/***/ },
/* 11 */
/***/ function(module, exports) {

	"use strict";
	var SignInDialogCtrl = (function () {
	    function SignInDialogCtrl(securityService) {
	        this.securityService = securityService;
	        this.fields = [
	            {
	                key: 'userName',
	                type: 'input',
	                templateOptions: {
	                    label: 'User name',
	                    required: true
	                }
	            },
	            {
	                key: 'password',
	                type: 'input',
	                templateOptions: {
	                    type: 'password',
	                    label: 'Password',
	                    required: true
	                }
	            }
	        ];
	    }
	    SignInDialogCtrl.prototype.login = function () {
	        var _this = this;
	        if (!this.form.$valid) {
	            return;
	        }
	        this.securityService
	            .signIn(this.model.userName, this.model.password)
	            .then(function () {
	            _this.modalInstance.close();
	        }, function () {
	            // TODO: create generic notification dialogs
	            alert('error');
	        });
	    };
	    SignInDialogCtrl.prototype.cancel = function () {
	        this.modalInstance.dismiss();
	    };
	    return SignInDialogCtrl;
	}());
	SignInDialogCtrl.$inject = ['securityService'];
	exports.SignInDialogCtrl = SignInDialogCtrl;


/***/ },
/* 12 */
/***/ function(module, exports) {

	"use strict";
	var ApplicationCtrl = (function () {
	    function ApplicationCtrl(securityUiService, busyIndicator, $state, $timeout, $scope) {
	        var _this = this;
	        this.securityUiService = securityUiService;
	        this.$state = $state;
	        this.busySpinnerIsVisible = false;
	        this.spinnerOptions = {
	            lines: 13,
	            length: 28,
	            width: 14,
	            radius: 35,
	            scale: 1,
	            corners: 1,
	            color: '#FFFFFF',
	            opacity: 0.25,
	            rotate: 0,
	            direction: 1,
	            speed: 2.2,
	            trail: 60,
	            fps: 20,
	            zIndex: 2e9,
	            className: 'spinner',
	            top: '50%',
	            left: '50%',
	            shadow: false,
	            hwaccel: false,
	            position: 'relative' // Element positioning
	        };
	        $scope.$watch(function () { return busyIndicator.isBusy; }, function () {
	            if (busyIndicator.isBusy) {
	                var showBusyIndicatorDelayInMilliseconds = 400;
	                $timeout(showBusyIndicatorDelayInMilliseconds)
	                    .then(function () {
	                    if (busyIndicator.isBusy) {
	                        _this.busySpinnerIsVisible = true;
	                    }
	                });
	            }
	            else {
	                _this.busySpinnerIsVisible = false;
	            }
	        });
	    }
	    ApplicationCtrl.prototype.searchAuctions = function () {
	        this.$state.go('auctionsSearch', { queryString: this.auctionsSearchQueryString });
	    };
	    return ApplicationCtrl;
	}());
	ApplicationCtrl.$inject = ['securityUiService', 'busyIndicator', '$state', '$timeout', '$scope'];
	exports.ApplicationCtrl = ApplicationCtrl;


/***/ },
/* 13 */
/***/ function(module, exports) {

	"use strict";
	var Routing = (function () {
	    function Routing() {
	    }
	    Routing.configure = function ($stateProvider, $urlRouterProvider) {
	        var states = [
	            {
	                name: 'home',
	                url: '/',
	                component: 'activeAuctionsList'
	            },
	            {
	                name: 'auctionsSearch',
	                url: '/auction/search/{queryString}',
	                component: 'activeAuctionsList',
	                resolve: {
	                    queryString: [
	                        '$stateParams', function ($stateParams) {
	                            return $stateParams['queryString'];
	                        }
	                    ]
	                }
	            },
	            {
	                name: 'createAuction',
	                url: '/auction/create',
	                component: 'createAuction'
	            },
	            {
	                name: 'displayAuction',
	                url: '/auction/{auctionId}',
	                component: 'displayAuction',
	                resolve: {
	                    auctionId: ['$stateParams', function ($stateParams) { return $stateParams['auctionId']; }]
	                }
	            },
	            {
	                name: 'userMessages',
	                url: '/user-messages',
	                component: 'userMessages'
	            },
	            {
	                name: 'userAuctionsList',
	                url: '/my-auctions',
	                component: 'userAuctionsList'
	            }
	        ];
	        for (var _i = 0, states_1 = states; _i < states_1.length; _i++) {
	            var state = states_1[_i];
	            $stateProvider.state(state);
	        }
	        $urlRouterProvider.when('', '/');
	    };
	    return Routing;
	}());
	exports.Routing = Routing;


/***/ },
/* 14 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var QueryHandler_1 = __webpack_require__(15);
	var GetSentUserMessagesQueryHandler = (function (_super) {
	    __extends(GetSentUserMessagesQueryHandler, _super);
	    function GetSentUserMessagesQueryHandler() {
	        return _super.apply(this, arguments) || this;
	    }
	    GetSentUserMessagesQueryHandler.prototype.getQueryName = function () {
	        return 'GetSentUserMessagesQuery';
	    };
	    return GetSentUserMessagesQueryHandler;
	}(QueryHandler_1.QueryHandler));
	exports.GetSentUserMessagesQueryHandler = GetSentUserMessagesQueryHandler;
	var GetUserInboxQueryHandler = (function (_super) {
	    __extends(GetUserInboxQueryHandler, _super);
	    function GetUserInboxQueryHandler() {
	        return _super.apply(this, arguments) || this;
	    }
	    GetUserInboxQueryHandler.prototype.getQueryName = function () {
	        return 'GetUserInboxQuery';
	    };
	    return GetUserInboxQueryHandler;
	}(QueryHandler_1.QueryHandler));
	exports.GetUserInboxQueryHandler = GetUserInboxQueryHandler;
	var GetAuctionDetailsQueryHandler = (function (_super) {
	    __extends(GetAuctionDetailsQueryHandler, _super);
	    function GetAuctionDetailsQueryHandler() {
	        return _super.apply(this, arguments) || this;
	    }
	    GetAuctionDetailsQueryHandler.prototype.getQueryName = function () {
	        return 'GetAuctionDetailsQuery';
	    };
	    return GetAuctionDetailsQueryHandler;
	}(QueryHandler_1.QueryHandler));
	exports.GetAuctionDetailsQueryHandler = GetAuctionDetailsQueryHandler;
	var GetAuctionsInvolvingUserQueryHandler = (function (_super) {
	    __extends(GetAuctionsInvolvingUserQueryHandler, _super);
	    function GetAuctionsInvolvingUserQueryHandler() {
	        return _super.apply(this, arguments) || this;
	    }
	    GetAuctionsInvolvingUserQueryHandler.prototype.getQueryName = function () {
	        return 'GetAuctionsInvolvingUserQuery';
	    };
	    return GetAuctionsInvolvingUserQueryHandler;
	}(QueryHandler_1.QueryHandler));
	exports.GetAuctionsInvolvingUserQueryHandler = GetAuctionsInvolvingUserQueryHandler;
	var SearchAuctionsQueryHandler = (function (_super) {
	    __extends(SearchAuctionsQueryHandler, _super);
	    function SearchAuctionsQueryHandler() {
	        return _super.apply(this, arguments) || this;
	    }
	    SearchAuctionsQueryHandler.prototype.getQueryName = function () {
	        return 'SearchAuctionsQuery';
	    };
	    return SearchAuctionsQueryHandler;
	}(QueryHandler_1.QueryHandler));
	exports.SearchAuctionsQueryHandler = SearchAuctionsQueryHandler;
	var AngularQueryHandlersRegistry = (function () {
	    function AngularQueryHandlersRegistry() {
	    }
	    return AngularQueryHandlersRegistry;
	}());
	AngularQueryHandlersRegistry.queryHandlers = {
	    'getSentUserMessagesQueryHandler': GetSentUserMessagesQueryHandler,
	    'getUserInboxQueryHandler': GetUserInboxQueryHandler,
	    'getAuctionDetailsQueryHandler': GetAuctionDetailsQueryHandler,
	    'getAuctionsInvolvingUserQueryHandler': GetAuctionsInvolvingUserQueryHandler,
	    'searchAuctionsQueryHandler': SearchAuctionsQueryHandler,
	};
	exports.AngularQueryHandlersRegistry = AngularQueryHandlersRegistry;


/***/ },
/* 15 */
/***/ function(module, exports) {

	"use strict";
	var QueryHandler = (function () {
	    function QueryHandler(httpService, qService) {
	        this.httpService = httpService;
	        this.qService = qService;
	    }
	    QueryHandler.prototype.handle = function (query) {
	        var url = "api/" + this.getQueryName() + "/Handle";
	        return this.httpService.get(url, { params: query }).then(function (response) { return response.data; });
	    };
	    return QueryHandler;
	}());
	QueryHandler.$inject = ['$http', '$q'];
	exports.QueryHandler = QueryHandler;


/***/ },
/* 16 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var DisplayAuctionCtrl_1 = __webpack_require__(17);
	var DisplayAuctionComponent = (function () {
	    function DisplayAuctionComponent() {
	        this.controller = DisplayAuctionCtrl_1.DisplayAuctionCtrl;
	        this.templateUrl = 'Template/Auctions/Display';
	        this.registerAs = 'displayAuction';
	        this.bindings = {
	            auctionId: '<'
	        };
	    }
	    return DisplayAuctionComponent;
	}());
	exports.DisplayAuctionComponent = DisplayAuctionComponent;


/***/ },
/* 17 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var GuidGenerator_1 = __webpack_require__(3);
	var CommandHandlingAsynchronityLevel_1 = __webpack_require__(4);
	var DisplayAuctionCtrl = (function () {
	    function DisplayAuctionCtrl(getAuctionDetailsQueryHandler, securityUiService, makeBidCommandUiHandler, genericModalService) {
	        var _this = this;
	        this.getAuctionDetailsQueryHandler = getAuctionDetailsQueryHandler;
	        this.securityUiService = securityUiService;
	        this.makeBidCommandUiHandler = makeBidCommandUiHandler;
	        this.genericModalService = genericModalService;
	        getAuctionDetailsQueryHandler.handle({
	            id: this.auctionId
	        })
	            .then(function (auction) {
	            _this.auctionLoadedCallback(auction);
	        });
	    }
	    DisplayAuctionCtrl.prototype.auctionLoadedCallback = function (auction) {
	        this.auction = auction;
	        this.bidPrice = auction.minimalPriceForNextBidder;
	    };
	    DisplayAuctionCtrl.prototype.makeUserEnteredBid = function () {
	        if (!_(this.bidPrice).isNumber()) {
	            this.genericModalService.showErrorNotification('Please enter a valid bid price.');
	            return;
	        }
	        else if (this.bidPrice < this.auction.minimalPriceForNextBidder) {
	            this.genericModalService
	                .showErrorNotification("Minimal bid price is " + this.auction.minimalPriceForNextBidder + ".");
	            return;
	        }
	        this.makeBid(this.bidPrice);
	    };
	    DisplayAuctionCtrl.prototype.makeBuyNowBid = function () {
	        if (!this.auction.buyNowPrice) {
	            throw new Error();
	        }
	        this.makeBid(this.auction.buyNowPrice);
	    };
	    DisplayAuctionCtrl.prototype.makeBid = function (bidPrice) {
	        var _this = this;
	        this.securityUiService.ensureUserIsAuthenticated()
	            .then(function () {
	            if (_this.securityUiService.currentUserName === _this.auction.createdByUserName) {
	                _this.genericModalService.showErrorNotification('You cannot bid at your own auction.');
	                return;
	            }
	            var makeBidCommand = {
	                auctionId: _this.auctionId,
	                expectedAuctionVersion: _this.auction.version,
	                price: bidPrice
	            };
	            _this.makeBidCommandUiHandler.handle(makeBidCommand, GuidGenerator_1.default.generateGuid(), CommandHandlingAsynchronityLevel_1.CommandHandlingAsynchronityLevel.WaitUnitReadModelIsUpdated)
	                .then(function () {
	                return _this.getAuctionDetailsQueryHandler.handle({
	                    id: _this.auctionId
	                });
	            })
	                .then(function (auction) {
	                var previousAuction = _this.auction;
	                _this.auctionLoadedCallback(auction);
	                if (auction.highestBidderUserName === _this.securityUiService.currentUserName) {
	                    if (auction.wasFinished) {
	                        _this.genericModalService
	                            .showSuccessNotification('Congratulations, you won the auction! Contact the seller in order to establish payment and delivery details.');
	                    }
	                    else if (auction.highestBidderUserName === previousAuction.highestBidderUserName) {
	                        _this.genericModalService.showSuccessNotification('You are still the highest bidder.');
	                    }
	                    else {
	                        _this.genericModalService.showSuccessNotification('Congratulations, you are now the highest bidder!');
	                    }
	                }
	                else {
	                    _this.genericModalService.showInformationNotification('Unfortunately your offer was not the highest.');
	                }
	            });
	        });
	    };
	    return DisplayAuctionCtrl;
	}());
	DisplayAuctionCtrl.$inject = ['getAuctionDetailsQueryHandler', 'securityUiService', 'makeBidCommandUiHandler', 'genericModalService'];
	exports.DisplayAuctionCtrl = DisplayAuctionCtrl;


/***/ },
/* 18 */
/***/ function(module, exports) {

	"use strict";
	var BusyIndicator = (function () {
	    function BusyIndicator(parentBusyIndicator) {
	        this.parentBusyIndicator = parentBusyIndicator;
	        this.busyStatesCount = 0;
	    }
	    Object.defineProperty(BusyIndicator.prototype, "isBusy", {
	        get: function () {
	            return this.busyStatesCount > 0 ||
	                (!!this.parentBusyIndicator && this.parentBusyIndicator.isBusy);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    BusyIndicator.prototype.enterBusyState = function () {
	        var _this = this;
	        var wasBusyStateLeft = false;
	        this.busyStatesCount++;
	        var leaveBusyStateFn = function () {
	            if (!wasBusyStateLeft) {
	                wasBusyStateLeft = true;
	                _this.busyStatesCount--;
	            }
	        };
	        return leaveBusyStateFn;
	    };
	    BusyIndicator.prototype.createNestedBusyIndicator = function () {
	        return new BusyIndicator(this);
	    };
	    BusyIndicator.prototype.attachToPromise = function (promise) {
	        var leaveBusyStateFn = this.enterBusyState();
	        promise.finally(function () {
	            leaveBusyStateFn();
	        });
	        return promise;
	    };
	    return BusyIndicator;
	}());
	Object.defineProperty(exports, "__esModule", { value: true });
	exports.default = BusyIndicator;


/***/ },
/* 19 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var CommandUiHandler_1 = __webpack_require__(20);
	var SendUserMessageCommandUiHandler = (function (_super) {
	    __extends(SendUserMessageCommandUiHandler, _super);
	    function SendUserMessageCommandUiHandler(sendUserMessageCommandHandler, busyIndicator, securityUiService, qService, genericModalService) {
	        var _this = _super.call(this, busyIndicator, securityUiService, qService, genericModalService) || this;
	        _this.sendUserMessageCommandHandler = sendUserMessageCommandHandler;
	        return _this;
	    }
	    SendUserMessageCommandUiHandler.prototype.getCommandHandler = function () {
	        return this.sendUserMessageCommandHandler;
	    };
	    return SendUserMessageCommandUiHandler;
	}(CommandUiHandler_1.CommandUiHandler));
	SendUserMessageCommandUiHandler.$inject = ['sendUserMessageCommandHandler', 'busyIndicator', 'securityUiService', '$q', 'genericModalService'];
	exports.SendUserMessageCommandUiHandler = SendUserMessageCommandUiHandler;
	var CreateAuctionCommandUiHandler = (function (_super) {
	    __extends(CreateAuctionCommandUiHandler, _super);
	    function CreateAuctionCommandUiHandler(createAuctionCommandHandler, busyIndicator, securityUiService, qService, genericModalService) {
	        var _this = _super.call(this, busyIndicator, securityUiService, qService, genericModalService) || this;
	        _this.createAuctionCommandHandler = createAuctionCommandHandler;
	        return _this;
	    }
	    CreateAuctionCommandUiHandler.prototype.getCommandHandler = function () {
	        return this.createAuctionCommandHandler;
	    };
	    return CreateAuctionCommandUiHandler;
	}(CommandUiHandler_1.CommandUiHandler));
	CreateAuctionCommandUiHandler.$inject = ['createAuctionCommandHandler', 'busyIndicator', 'securityUiService', '$q', 'genericModalService'];
	exports.CreateAuctionCommandUiHandler = CreateAuctionCommandUiHandler;
	var FinishAuctionCommandUiHandler = (function (_super) {
	    __extends(FinishAuctionCommandUiHandler, _super);
	    function FinishAuctionCommandUiHandler(finishAuctionCommandHandler, busyIndicator, securityUiService, qService, genericModalService) {
	        var _this = _super.call(this, busyIndicator, securityUiService, qService, genericModalService) || this;
	        _this.finishAuctionCommandHandler = finishAuctionCommandHandler;
	        return _this;
	    }
	    FinishAuctionCommandUiHandler.prototype.getCommandHandler = function () {
	        return this.finishAuctionCommandHandler;
	    };
	    return FinishAuctionCommandUiHandler;
	}(CommandUiHandler_1.CommandUiHandler));
	FinishAuctionCommandUiHandler.$inject = ['finishAuctionCommandHandler', 'busyIndicator', 'securityUiService', '$q', 'genericModalService'];
	exports.FinishAuctionCommandUiHandler = FinishAuctionCommandUiHandler;
	var MakeBidCommandUiHandler = (function (_super) {
	    __extends(MakeBidCommandUiHandler, _super);
	    function MakeBidCommandUiHandler(makeBidCommandHandler, busyIndicator, securityUiService, qService, genericModalService) {
	        var _this = _super.call(this, busyIndicator, securityUiService, qService, genericModalService) || this;
	        _this.makeBidCommandHandler = makeBidCommandHandler;
	        return _this;
	    }
	    MakeBidCommandUiHandler.prototype.getCommandHandler = function () {
	        return this.makeBidCommandHandler;
	    };
	    return MakeBidCommandUiHandler;
	}(CommandUiHandler_1.CommandUiHandler));
	MakeBidCommandUiHandler.$inject = ['makeBidCommandHandler', 'busyIndicator', 'securityUiService', '$q', 'genericModalService'];
	exports.MakeBidCommandUiHandler = MakeBidCommandUiHandler;
	var AngularCommandUiHandlersRegistry = (function () {
	    function AngularCommandUiHandlersRegistry() {
	    }
	    return AngularCommandUiHandlersRegistry;
	}());
	AngularCommandUiHandlersRegistry.commandUiHandlers = {
	    'sendUserMessageCommandUiHandler': SendUserMessageCommandUiHandler,
	    'createAuctionCommandUiHandler': CreateAuctionCommandUiHandler,
	    'finishAuctionCommandUiHandler': FinishAuctionCommandUiHandler,
	    'makeBidCommandUiHandler': MakeBidCommandUiHandler,
	};
	exports.AngularCommandUiHandlersRegistry = AngularCommandUiHandlersRegistry;


/***/ },
/* 20 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var CommandHandlingErrorType_1 = __webpack_require__(7);
	var NotificationType_1 = __webpack_require__(21);
	var CommandUiHandler = (function () {
	    function CommandUiHandler(busyIndicator, securityUiService, qService, genericModalService) {
	        this.busyIndicator = busyIndicator;
	        this.securityUiService = securityUiService;
	        this.qService = qService;
	        this.genericModalService = genericModalService;
	        this.$inject = ['busyIndicator', 'securityUiService', '$q', 'genericModalService'];
	    }
	    CommandUiHandler.prototype.handle = function (command, commandId, asynchronityLevel) {
	        var _this = this;
	        return this.securityUiService.ensureUserIsAuthenticated()
	            .then(function () {
	            var promise = _this.getCommandHandler().handle(command, commandId, asynchronityLevel);
	            return _this.busyIndicator.attachToPromise(promise)
	                .catch(function (commandHandlingErrorType) {
	                var actionData = _this.getActionDataForCommandHandlingErrorType(commandHandlingErrorType);
	                _this.genericModalService
	                    .showNotification(actionData.notificationMessage, actionData.notificationType);
	                return _this.qService.reject();
	            });
	        });
	    };
	    CommandUiHandler.prototype.getActionDataForCommandHandlingErrorType = function (commandHandlingErrorType) {
	        switch (commandHandlingErrorType) {
	            case CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToConnectToFeedbackHub:
	            case CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToQueue:
	            case CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToProcess:
	                return {
	                    notificationType: NotificationType_1.NotificationType.Error,
	                    notificationMessage: 'Failed to process you request. Please try again.'
	                };
	            case CommandHandlingErrorType_1.CommandHandlingErrorType.ProcessingTimeout:
	                return {
	                    notificationType: NotificationType_1.NotificationType.Error,
	                    notificationMessage: 'Your request timed out. Please check whether its effects are visible or try again.'
	                };
	            case CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToSubscribeToReadModelChangeNotification:
	            case CommandHandlingErrorType_1.CommandHandlingErrorType.ReadModelChangeNotificationTimeout:
	                return {
	                    notificationType: NotificationType_1.NotificationType.Information,
	                    notificationMessage: 'Your request was handled, but its effects may not be visible for a while.'
	                };
	            default:
	                return {
	                    notificationType: NotificationType_1.NotificationType.Error,
	                    notificationMessage: 'An unknown error occurred.'
	                };
	        }
	    };
	    return CommandUiHandler;
	}());
	exports.CommandUiHandler = CommandUiHandler;


/***/ },
/* 21 */
/***/ function(module, exports) {

	"use strict";
	var NotificationType;
	(function (NotificationType) {
	    NotificationType[NotificationType["Information"] = 0] = "Information";
	    NotificationType[NotificationType["Success"] = 1] = "Success";
	    NotificationType[NotificationType["Error"] = 2] = "Error";
	})(NotificationType = exports.NotificationType || (exports.NotificationType = {}));


/***/ },
/* 22 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var AuctionsListCtrl_1 = __webpack_require__(23);
	var AuctionsListComponent = (function () {
	    function AuctionsListComponent() {
	        this.controller = AuctionsListCtrl_1.AuctionsListCtrl;
	        this.templateUrl = 'Template/Auctions/List';
	        this.registerAs = 'auctionsList';
	        this.bindings = {
	            getAuctions: '<',
	            displayedColumns: '<',
	            onReloadFunctionChanged: '&'
	        };
	    }
	    return AuctionsListComponent;
	}());
	exports.AuctionsListComponent = AuctionsListComponent;


/***/ },
/* 23 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var ListHeaderDefinition_1 = __webpack_require__(24);
	var ListCtrl_1 = __webpack_require__(25);
	var AuctionsListCtrl = (function (_super) {
	    __extends(AuctionsListCtrl, _super);
	    function AuctionsListCtrl(scope) {
	        return _super.call(this, scope) || this;
	    }
	    AuctionsListCtrl.prototype.getAllHeaderDefinitions = function () {
	        return [
	            new ListHeaderDefinition_1.default('TitleAndDescription', 'Auction'),
	            new ListHeaderDefinition_1.default('CurrentPrice', 'Current price', { width: '120px', 'text-align': 'right' }),
	            new ListHeaderDefinition_1.default('SoldFor', 'Sold for', { width: '120px', 'text-align': 'right' }),
	            new ListHeaderDefinition_1.default('BuyNowPrice', 'Buy now price', { width: '120px', 'text-align': 'right' }),
	            new ListHeaderDefinition_1.default('NumberOfBids', 'Bids', { width: '50px', 'text-align': 'right' }),
	            new ListHeaderDefinition_1.default('Seller', 'Seller', { width: '150px' }),
	            new ListHeaderDefinition_1.default('Winner', 'Winner', { width: '150px' }),
	            new ListHeaderDefinition_1.default('StartedDateTime', 'Started', { width: '180px' }),
	            new ListHeaderDefinition_1.default('EndsDateTime', 'Ends in', { width: '130px' }),
	            new ListHeaderDefinition_1.default('EndedDateTime', 'Ended', { width: '180px' }),
	        ];
	    };
	    AuctionsListCtrl.prototype.getResults = function (pageSize, pageNumber) {
	        return this.getAuctions(pageSize, pageNumber);
	    };
	    return AuctionsListCtrl;
	}(ListCtrl_1.ListCtrl));
	AuctionsListCtrl.$inject = ['$scope'];
	exports.AuctionsListCtrl = AuctionsListCtrl;


/***/ },
/* 24 */
/***/ function(module, exports) {

	"use strict";
	var ListHeaderDefinition = (function () {
	    function ListHeaderDefinition(column, displayName, style) {
	        this.column = column;
	        this.displayName = displayName;
	        this.style = style;
	    }
	    return ListHeaderDefinition;
	}());
	Object.defineProperty(exports, "__esModule", { value: true });
	exports.default = ListHeaderDefinition;


/***/ },
/* 25 */
/***/ function(module, exports) {

	"use strict";
	var ListCtrl = (function () {
	    function ListCtrl(scope) {
	        var _this = this;
	        this.reload = angular.noop;
	        this.tastyInitCfg = {
	            'count': 25,
	            'page': 1
	        };
	        this.getResource = function (paramsString, paramsObject) {
	            var pageSize = paramsObject.count;
	            var pageNumber = paramsObject.page;
	            return _this.getResults(pageSize, pageNumber)
	                .then(function (pagedResults) {
	                var displayedHeaders = _(_this.getAllHeaderDefinitions())
	                    .filter(function (headerDefinition) { return _(_this.displayedColumns).contains(headerDefinition.column); });
	                var tastyHeader = _(displayedHeaders).map(function (header) { return _this.mapHeaderDefinitionToTastyTableHeader(header); });
	                return {
	                    rows: pagedResults.pageItems,
	                    pagination: {
	                        count: pagedResults.pageSize,
	                        page: pagedResults.pageNumber,
	                        pages: pagedResults.totalPagesCount,
	                        size: pagedResults.totalItemsCount
	                    },
	                    header: tastyHeader
	                };
	            });
	        };
	        scope.$watch(function () { return _this.reload; }, function () {
	            if (_this.onReloadFunctionChanged) {
	                _this.onReloadFunctionChanged({ reloadFunction: _this.reload });
	            }
	            _this.reload();
	        });
	    }
	    ListCtrl.prototype.checkIfColumnIsDisplayed = function (column) {
	        return _(this.displayedColumns).contains(column);
	    };
	    ListCtrl.prototype.mapHeaderDefinitionToTastyTableHeader = function (listHeaderDefinition) {
	        return {
	            key: listHeaderDefinition.column,
	            name: listHeaderDefinition.displayName,
	            style: listHeaderDefinition.style
	        };
	    };
	    return ListCtrl;
	}());
	ListCtrl.$inject = ['$scope'];
	exports.ListCtrl = ListCtrl;


/***/ },
/* 26 */
/***/ function(module, exports) {

	"use strict";
	var BusyIndicatingHttpInterceptor = (function () {
	    function BusyIndicatingHttpInterceptor(busyIndicator, $q) {
	        var _this = this;
	        this.busyIndicator = busyIndicator;
	        this.$q = $q;
	        this.numberOfRequestsInProgress = 0;
	        this.request = function (config) {
	            if (_this.numberOfRequestsInProgress === 0) {
	                _this.leaveHttpBusyStateFn = _this.busyIndicator.enterBusyState();
	            }
	            _this.numberOfRequestsInProgress++;
	            return config;
	        };
	        this.response = function (response) {
	            _this.decrementNumberOfRequestsInProgress();
	            return response;
	        };
	        this.responseError = function (rejection) {
	            _this.decrementNumberOfRequestsInProgress();
	            return _this.$q.reject(rejection);
	        };
	    }
	    BusyIndicatingHttpInterceptor.prototype.decrementNumberOfRequestsInProgress = function () {
	        this.numberOfRequestsInProgress--;
	        if (this.numberOfRequestsInProgress === 0) {
	            this.leaveHttpBusyStateFn();
	        }
	    };
	    return BusyIndicatingHttpInterceptor;
	}());
	BusyIndicatingHttpInterceptor.$inject = ['busyIndicator', '$q'];
	Object.defineProperty(exports, "__esModule", { value: true });
	exports.default = BusyIndicatingHttpInterceptor;


/***/ },
/* 27 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var SimpleNotificationDialogCtrl_1 = __webpack_require__(28);
	var SimpleNotificationDialogComponent = (function () {
	    function SimpleNotificationDialogComponent() {
	        this.controller = SimpleNotificationDialogCtrl_1.SimpleNotificationDialogCtrl;
	        this.templateUrl = 'Template/Shared/SimpleNotificationDialog';
	        this.registerAs = 'simpleNotificationDialog';
	        this.bindings = {
	            modalInstance: '<',
	            resolve: '<'
	        };
	    }
	    return SimpleNotificationDialogComponent;
	}());
	exports.SimpleNotificationDialogComponent = SimpleNotificationDialogComponent;


/***/ },
/* 28 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var NotificationType_1 = __webpack_require__(21);
	var SimpleNotificationDialogCtrl = (function () {
	    function SimpleNotificationDialogCtrl() {
	    }
	    Object.defineProperty(SimpleNotificationDialogCtrl.prototype, "isInformationNotification", {
	        get: function () {
	            return this.resolve.notificationType === NotificationType_1.NotificationType.Information;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(SimpleNotificationDialogCtrl.prototype, "isSuccessNotification", {
	        get: function () {
	            return this.resolve.notificationType === NotificationType_1.NotificationType.Success;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(SimpleNotificationDialogCtrl.prototype, "isErrorNotification", {
	        get: function () {
	            return this.resolve.notificationType === NotificationType_1.NotificationType.Error;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    return SimpleNotificationDialogCtrl;
	}());
	exports.SimpleNotificationDialogCtrl = SimpleNotificationDialogCtrl;


/***/ },
/* 29 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var NotificationType_1 = __webpack_require__(21);
	var GenericModalService = (function () {
	    function GenericModalService(modalService) {
	        this.modalService = modalService;
	    }
	    GenericModalService.prototype.showInformationNotification = function (notificationMessage) {
	        return this.showNotification(notificationMessage, NotificationType_1.NotificationType.Information);
	    };
	    GenericModalService.prototype.showSuccessNotification = function (notificationMessage) {
	        return this.showNotification(notificationMessage, NotificationType_1.NotificationType.Success);
	    };
	    GenericModalService.prototype.showErrorNotification = function (notificationMessage) {
	        return this.showNotification(notificationMessage, NotificationType_1.NotificationType.Error);
	    };
	    GenericModalService.prototype.showNotification = function (notificationMessage, notificationType) {
	        var modalInstance = this.modalService.open({
	            component: 'simpleNotificationDialog',
	            resolve: {
	                notificationMessage: function () { return notificationMessage; },
	                notificationType: function () { return notificationType; }
	            }
	        });
	        return modalInstance.result;
	    };
	    return GenericModalService;
	}());
	GenericModalService.$inject = ['$uibModal'];
	Object.defineProperty(exports, "__esModule", { value: true });
	exports.default = GenericModalService;


/***/ },
/* 30 */
/***/ function(module, exports) {

	"use strict";
	var Configuration = (function () {
	    function Configuration() {
	        this.commandHandlingTimeoutMilliseconds = 10 * 1000;
	        this.readModelChangeNotificationTimeoutMilliseconds = 10 * 1000;
	    }
	    return Configuration;
	}());
	Object.defineProperty(exports, "__esModule", { value: true });
	exports.default = Configuration;


/***/ },
/* 31 */
/***/ function(module, exports) {

	"use strict";
	var FormatDateTimeFilterFactory = (function () {
	    function FormatDateTimeFilterFactory() {
	    }
	    FormatDateTimeFilterFactory.createStandardFilterFunction = function () {
	        return function (value) {
	            if (!value) {
	                return null;
	            }
	            return moment(value).format('Do MMM YYYY, h:mm A');
	        };
	    };
	    FormatDateTimeFilterFactory.createToNowFilterFunction = function () {
	        return function (value) {
	            if (!value) {
	                return null;
	            }
	            return moment(value).toNow(true);
	        };
	    };
	    return FormatDateTimeFilterFactory;
	}());
	FormatDateTimeFilterFactory.$inject = [''];
	Object.defineProperty(exports, "__esModule", { value: true });
	exports.default = FormatDateTimeFilterFactory;


/***/ },
/* 32 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var ComposeUserMessageDialogCtrl_1 = __webpack_require__(33);
	var ComposeUserMessageDialogComponent = (function () {
	    function ComposeUserMessageDialogComponent() {
	        this.controller = ComposeUserMessageDialogCtrl_1.ComposeUserMessageDialogCtrl;
	        this.templateUrl = 'Template/UserMessaging/ComposeMessageDialog';
	        this.registerAs = 'composeUserMessageDialog';
	        this.bindings = {
	            modalInstance: '<',
	            resolve: '<'
	        };
	    }
	    return ComposeUserMessageDialogComponent;
	}());
	exports.ComposeUserMessageDialogComponent = ComposeUserMessageDialogComponent;


/***/ },
/* 33 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var GuidGenerator_1 = __webpack_require__(3);
	var CommandHandlingAsynchronityLevel_1 = __webpack_require__(4);
	var ComposeUserMessageDialogCtrl = (function () {
	    function ComposeUserMessageDialogCtrl(sendUserMessageCommandUiHandler, genericModalService) {
	        this.sendUserMessageCommandUiHandler = sendUserMessageCommandUiHandler;
	        this.genericModalService = genericModalService;
	        this.sendUserMessageCommandId = GuidGenerator_1.default.generateGuid();
	        this.model = {
	            recipientUserName: '',
	            messageSubject: '',
	            messageBody: ''
	        };
	        this.model.recipientUserName = this.resolve.recipientUserName;
	        this.model.messageSubject = this.resolve.messageSubject || '';
	        this.fields = [
	            {
	                key: 'messageSubject',
	                type: 'input',
	                templateOptions: {
	                    label: 'Subject',
	                    required: true
	                }
	            },
	            {
	                key: 'messageBody',
	                type: 'textarea',
	                templateOptions: {
	                    label: 'Message',
	                    required: true,
	                    maxlength: 10000,
	                    rows: 8
	                }
	            },
	        ];
	    }
	    ComposeUserMessageDialogCtrl.prototype.sendMessage = function () {
	        var _this = this;
	        if (!this.form.$valid) {
	            return;
	        }
	        this.sendUserMessageCommandUiHandler.handle(this.model, this.sendUserMessageCommandId, CommandHandlingAsynchronityLevel_1.CommandHandlingAsynchronityLevel.WaitUntilCommandIsProcessed)
	            .then(function () {
	            _this.modalInstance.close();
	            _this.genericModalService.showSuccessNotification('Your message was sent successfully.');
	        });
	    };
	    ComposeUserMessageDialogCtrl.prototype.cancel = function () {
	        this.modalInstance.dismiss();
	    };
	    return ComposeUserMessageDialogCtrl;
	}());
	ComposeUserMessageDialogCtrl.$inject = ['sendUserMessageCommandUiHandler', 'genericModalService'];
	exports.ComposeUserMessageDialogCtrl = ComposeUserMessageDialogCtrl;


/***/ },
/* 34 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var UserReferenceCtrl_1 = __webpack_require__(35);
	var UserReferenceComponent = (function () {
	    function UserReferenceComponent() {
	        this.controller = UserReferenceCtrl_1.UserReferenceCtrl;
	        this.templateUrl = 'Template/Shared/UserReference';
	        this.registerAs = 'userReference';
	        this.bindings = {
	            userName: '<'
	        };
	    }
	    return UserReferenceComponent;
	}());
	exports.UserReferenceComponent = UserReferenceComponent;


/***/ },
/* 35 */
/***/ function(module, exports) {

	"use strict";
	var UserReferenceCtrl = (function () {
	    function UserReferenceCtrl(modalService, securityUiService) {
	        this.modalService = modalService;
	        this.securityUiService = securityUiService;
	    }
	    UserReferenceCtrl.prototype.opendMessageCompositionDialog = function () {
	        var _this = this;
	        this.modalService.open({
	            component: 'composeUserMessageDialog',
	            resolve: {
	                recipientUserName: function () { return _this.userName; }
	            }
	        });
	    };
	    UserReferenceCtrl.prototype.checkIfUserIsCurrentUser = function () {
	        return this.userName === this.securityUiService.currentUserName;
	    };
	    return UserReferenceCtrl;
	}());
	UserReferenceCtrl.$inject = ['$uibModal', 'securityUiService'];
	exports.UserReferenceCtrl = UserReferenceCtrl;


/***/ },
/* 36 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var UserMessagesCtrl_1 = __webpack_require__(37);
	var UserMessagesComponent = (function () {
	    function UserMessagesComponent() {
	        this.controller = UserMessagesCtrl_1.UserMessagesCtrl;
	        this.templateUrl = 'Template/UserMessaging/UserMessages';
	        this.registerAs = 'userMessages';
	    }
	    return UserMessagesComponent;
	}());
	exports.UserMessagesComponent = UserMessagesComponent;


/***/ },
/* 37 */
/***/ function(module, exports) {

	"use strict";
	var UserMessagesCtrl = (function () {
	    function UserMessagesCtrl(getUserInboxQueryHandler, getSentUserMessagesQueryHandler) {
	        var _this = this;
	        this.getUserInboxQueryHandler = getUserInboxQueryHandler;
	        this.getSentUserMessagesQueryHandler = getSentUserMessagesQueryHandler;
	        this.inboxMessagesListDisplayedColumns = ['SenderUserName', 'SubjectAndBody', 'SentDateTime'];
	        this.sentMessagesListDisplayedColumns = ['SubjectAndBody', 'SentDateTime', 'RecipientUserName'];
	        this.getUserInbox = function (pageSize, pageNumber) {
	            var query = {
	                pageSize: pageSize,
	                pageNumber: pageNumber
	            };
	            return _this.getUserInboxQueryHandler.handle(query);
	        };
	        this.getSentUserMessages = function (pageSize, pageNumber) {
	            var query = {
	                pageSize: pageSize,
	                pageNumber: pageNumber
	            };
	            return _this.getSentUserMessagesQueryHandler.handle(query);
	        };
	    }
	    return UserMessagesCtrl;
	}());
	UserMessagesCtrl.$inject = ['getUserInboxQueryHandler', 'getSentUserMessagesQueryHandler'];
	exports.UserMessagesCtrl = UserMessagesCtrl;


/***/ },
/* 38 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var UserAuctionsListCtrl_1 = __webpack_require__(39);
	var UserAuctionsListComponent = (function () {
	    function UserAuctionsListComponent() {
	        this.controller = UserAuctionsListCtrl_1.UserAuctionsListCtrl;
	        this.templateUrl = 'Template/Auctions/UserList';
	        this.registerAs = 'userAuctionsList';
	        //bindings = {
	        //	queryString: '<'
	        //   }
	    }
	    return UserAuctionsListComponent;
	}());
	exports.UserAuctionsListComponent = UserAuctionsListComponent;


/***/ },
/* 39 */
/***/ function(module, exports) {

	"use strict";
	var UserAuctionsListCtrl = (function () {
	    function UserAuctionsListCtrl(getAuctionsInvolvingUserQueryHandler) {
	        var _this = this;
	        this.getAuctionsInvolvingUserQueryHandler = getAuctionsInvolvingUserQueryHandler;
	        this.userInvolvementIntoAuction = 'Selling';
	        this.getAuctions = function (pageSize, pageNumber) {
	            _this.refreshDisplayedColumns();
	            var query = {
	                queryString: _this.queryString,
	                userInvolvementIntoAuction: _this.userInvolvementIntoAuction,
	                pageSize: pageSize,
	                pageNumber: pageNumber
	            };
	            return _this.getAuctionsInvolvingUserQueryHandler.handle(query);
	        };
	    }
	    UserAuctionsListCtrl.prototype.setReloadFunction = function (reloadFn) {
	        this.search = reloadFn;
	    };
	    UserAuctionsListCtrl.prototype.refreshDisplayedColumns = function () {
	        var commonColumns = ['TitleAndDescription', 'BuyNowPrice'];
	        // TODO: add ended date and final price columns
	        var userInvolvementIntoAuctionToAdditionalColumnsMap = {
	            'Selling': ['CurrentPrice', 'BuyNowPrice', 'NumberOfBids', 'EndsDateTime'],
	            'Sold': ['SoldFor', 'Winner', 'BuyNowPrice', 'NumberOfBids', 'EndedDateTime'],
	            'FailedToSell': ['EndedDateTime'],
	            'Bidding': ['Seller', 'CurrentPrice', 'NumberOfBids', 'EndsDateTime'],
	            'Bought': ['SoldFor', 'Seller', 'NumberOfBids', 'EndedDateTime'],
	            'FailedToBuy': ['SoldFor', 'Seller', 'Winner', 'NumberOfBids', 'EndedDateTime']
	        };
	        this.displayedColumns = commonColumns
	            .concat(userInvolvementIntoAuctionToAdditionalColumnsMap[this.userInvolvementIntoAuction]);
	    };
	    return UserAuctionsListCtrl;
	}());
	UserAuctionsListCtrl.$inject = ['getAuctionsInvolvingUserQueryHandler'];
	exports.UserAuctionsListCtrl = UserAuctionsListCtrl;


/***/ },
/* 40 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var ActiveAuctionsListCtrl_1 = __webpack_require__(41);
	var ActiveAuctionsListComponent = (function () {
	    function ActiveAuctionsListComponent() {
	        this.controller = ActiveAuctionsListCtrl_1.ActiveAuctionsListCtrl;
	        this.templateUrl = 'Template/Auctions/ActiveList';
	        this.registerAs = 'activeAuctionsList';
	        this.bindings = {
	            queryString: '<'
	        };
	    }
	    return ActiveAuctionsListComponent;
	}());
	exports.ActiveAuctionsListComponent = ActiveAuctionsListComponent;


/***/ },
/* 41 */
/***/ function(module, exports) {

	"use strict";
	var ActiveAuctionsListCtrl = (function () {
	    function ActiveAuctionsListCtrl(searchAuctionsQueryHandler) {
	        var _this = this;
	        this.searchAuctionsQueryHandler = searchAuctionsQueryHandler;
	        this.displayedColumns = [
	            'TitleAndDescription', 'CurrentPrice', 'BuyNowPrice', 'NumberOfBids', 'Seller', 'EndsDateTime'
	        ];
	        this.getAuctions = function (pageSize, pageNumber) {
	            var query = {
	                queryString: _this.queryString,
	                pageSize: pageSize,
	                pageNumber: pageNumber
	            };
	            return _this.searchAuctionsQueryHandler.handle(query);
	        };
	    }
	    return ActiveAuctionsListCtrl;
	}());
	ActiveAuctionsListCtrl.$inject = ['searchAuctionsQueryHandler'];
	exports.ActiveAuctionsListCtrl = ActiveAuctionsListCtrl;


/***/ },
/* 42 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var UserMessagesListCtrl_1 = __webpack_require__(43);
	var UserMessagesListComponent = (function () {
	    function UserMessagesListComponent() {
	        this.controller = UserMessagesListCtrl_1.UserMessagesListCtrl;
	        this.templateUrl = 'Template/UserMessaging/UserMessagesList';
	        this.registerAs = 'userMessagesList';
	        this.bindings = {
	            getMessages: '<',
	            displayedColumns: '<'
	        };
	    }
	    return UserMessagesListComponent;
	}());
	exports.UserMessagesListComponent = UserMessagesListComponent;


/***/ },
/* 43 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var ListCtrl_1 = __webpack_require__(25);
	var ListHeaderDefinition_1 = __webpack_require__(24);
	var UserMessagesListCtrl = (function (_super) {
	    __extends(UserMessagesListCtrl, _super);
	    function UserMessagesListCtrl(scope, modalService) {
	        var _this = _super.call(this, scope) || this;
	        _this.scope = scope;
	        _this.modalService = modalService;
	        return _this;
	    }
	    UserMessagesListCtrl.prototype.getAllHeaderDefinitions = function () {
	        return [
	            new ListHeaderDefinition_1.default('SenderUserName', 'From', { width: '200px' }),
	            new ListHeaderDefinition_1.default('RecipientUserName', 'To', { width: '200px' }),
	            new ListHeaderDefinition_1.default('SubjectAndBody', 'Message'),
	            new ListHeaderDefinition_1.default('SentDateTime', 'Sent', { width: '180px' })
	        ];
	    };
	    UserMessagesListCtrl.prototype.getResults = function (pageSize, pageNumber) {
	        return this.getMessages(pageSize, pageNumber);
	    };
	    UserMessagesListCtrl.prototype.displayUserMessage = function (userMessage) {
	        this.modalService.open({
	            component: 'displayUserMessageDialog',
	            resolve: {
	                userMessage: function () { return userMessage; }
	            }
	        });
	    };
	    return UserMessagesListCtrl;
	}(ListCtrl_1.ListCtrl));
	UserMessagesListCtrl.$inject = ['$scope', '$uibModal'];
	exports.UserMessagesListCtrl = UserMessagesListCtrl;


/***/ },
/* 44 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var NewLinesToParagraphsCtrl_1 = __webpack_require__(45);
	var NewLinesToParagraphsComponent = (function () {
	    function NewLinesToParagraphsComponent() {
	        this.controller = NewLinesToParagraphsCtrl_1.NewLinesToParagraphsCtrl;
	        this.registerAs = 'newLinesToParagraphs';
	        this.bindings = {
	            text: '<'
	        };
	    }
	    return NewLinesToParagraphsComponent;
	}());
	exports.NewLinesToParagraphsComponent = NewLinesToParagraphsComponent;


/***/ },
/* 45 */
/***/ function(module, exports) {

	"use strict";
	var NewLinesToParagraphsCtrl = (function () {
	    function NewLinesToParagraphsCtrl(element, scope) {
	        var _this = this;
	        scope.$watch(function () { return _this.text; }, function () {
	            element.empty();
	            if (!_(_this.text).isString()) {
	                return;
	            }
	            var lines = _this.text.split(/\r\n|\r|\n/);
	            var paragraphElements = _(lines).map(function (line) { return $('<p></p>').text(line); });
	            for (var _i = 0, paragraphElements_1 = paragraphElements; _i < paragraphElements_1.length; _i++) {
	                var paragraphElement = paragraphElements_1[_i];
	                element.append(paragraphElement);
	            }
	        });
	    }
	    return NewLinesToParagraphsCtrl;
	}());
	NewLinesToParagraphsCtrl.$inject = ['$element', '$scope'];
	exports.NewLinesToParagraphsCtrl = NewLinesToParagraphsCtrl;


/***/ },
/* 46 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var DisplayUserMessageDialogCtrl_1 = __webpack_require__(47);
	var DisplayUserMessageDialogComponent = (function () {
	    function DisplayUserMessageDialogComponent() {
	        this.controller = DisplayUserMessageDialogCtrl_1.DisplayUserMessageDialogCtrl;
	        this.templateUrl = 'Template/UserMessaging/DisplayMessageDialog';
	        this.registerAs = 'displayUserMessageDialog';
	        this.bindings = {
	            modalInstance: '<',
	            resolve: '<'
	        };
	    }
	    return DisplayUserMessageDialogComponent;
	}());
	exports.DisplayUserMessageDialogComponent = DisplayUserMessageDialogComponent;


/***/ },
/* 47 */
/***/ function(module, exports) {

	"use strict";
	var DisplayUserMessageDialogCtrl = (function () {
	    function DisplayUserMessageDialogCtrl(modalService, securityUiService) {
	        this.modalService = modalService;
	        this.securityUiService = securityUiService;
	    }
	    Object.defineProperty(DisplayUserMessageDialogCtrl.prototype, "messageIsFromCurrentUser", {
	        get: function () {
	            return this.securityUiService.currentUserName === this.resolve.userMessage.senderUserName;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(DisplayUserMessageDialogCtrl.prototype, "messageIsToCurrentUser", {
	        get: function () {
	            return this.securityUiService.currentUserName === this.resolve.userMessage.recipientUserName;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    DisplayUserMessageDialogCtrl.prototype.reply = function () {
	        var _this = this;
	        this.modalService.open({
	            component: 'composeUserMessageDialog',
	            resolve: {
	                recipientUserName: function () { return _this.resolve.userMessage.senderUserName; },
	                messageSubject: function () { return "RE: " + _this.resolve.userMessage.subject; }
	            }
	        });
	    };
	    DisplayUserMessageDialogCtrl.prototype.close = function () {
	        this.modalInstance.dismiss();
	    };
	    return DisplayUserMessageDialogCtrl;
	}());
	DisplayUserMessageDialogCtrl.$inject = ['$uibModal', 'securityUiService'];
	exports.DisplayUserMessageDialogCtrl = DisplayUserMessageDialogCtrl;


/***/ }
/******/ ]);
//# sourceMappingURL=ApplicationBundle.js.map