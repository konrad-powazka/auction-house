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
	var GeneratedCommandHandlers_1 = __webpack_require__(4);
	var Application = (function () {
	    function Application() {
	    }
	    Application.bootstrap = function () {
	        var module = angular.module('auctionHouse', [
	            'ui.router', 'formly', 'formlyBootstrap', 'ngMessages', 'ngAnimate', 'ui.bootstrap',
	            'ui.bootstrap.datetimepicker'
	        ]);
	        module.service(GeneratedCommandHandlers_1.AngularCommandHandlersRegistry.commandHandlers);
	        for (var _i = 0, _a = Application.components; _i < _a.length; _i++) {
	            var component = _a[_i];
	            module.component(component.registerAs, component);
	        }
	        Application.configureModule.$inject = ['$stateProvider'];
	        module.config(Application.configureModule);
	        Application.runModule.$inject = ['formlyConfig', 'formlyValidationMessages'];
	        module.run(Application.runModule);
	    };
	    ;
	    Application.configureModule = function ($stateProvider) {
	        Application.configureRouting($stateProvider);
	    };
	    Application.runModule = function (formlyConfig, formlyValidationMessages) {
	        Application.configureFormly(formlyConfig, formlyValidationMessages);
	    };
	    Application.configureRouting = function ($stateProvider) {
	        var states = [
	            {
	                name: 'createAuction',
	                url: '/auction/create',
	                component: 'createAuction'
	            }
	        ];
	        for (var _i = 0, states_1 = states; _i < states_1.length; _i++) {
	            var state = states_1[_i];
	            $stateProvider.state(state);
	        }
	    };
	    ;
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
	        formlyConfig.extras.errorExistsAndShouldBeVisibleExpression = 'fc.$touched || form.$submitted';
	    };
	    ;
	    return Application;
	}());
	Application.components = [
	    new CreateAuctionComponent_1.CreateAuctionComponent()
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
	var CommandHandlingErrorType_1 = __webpack_require__(3);
	var CreateAuctionCtrl = (function () {
	    function CreateAuctionCtrl(createAuctionCommandHandler) {
	        this.createAuctionCommandHandler = createAuctionCommandHandler;
	        this.model = {
	            id: this.guid(),
	            title: '',
	            description: '',
	            auctionId: this.guid(),
	            startingPrice: 5,
	            buyNowPrice: 10,
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
	                    maxlength: 10000
	                }
	            },
	            {
	                key: 'endDate',
	                type: 'dateTimePicker',
	                templateOptions: {
	                    label: 'End date and time',
	                    required: true
	                }
	            }
	        ];
	    }
	    CreateAuctionCtrl.prototype.submit = function () {
	        if (!this.form.$valid) {
	            return;
	        }
	        this.createAuctionCommandHandler
	            .handle(this.model)
	            .then(function () { return alert('Success'); })
	            .catch(function (commandHandlingErrorType) {
	            return alert("Command processing error: " + CommandHandlingErrorType_1.CommandHandlingErrorType[commandHandlingErrorType]);
	        });
	    };
	    CreateAuctionCtrl.prototype.guid = function () {
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
	    return CreateAuctionCtrl;
	}()); //
	CreateAuctionCtrl.$inject = ['CreateAuctionCommandHandler'];
	exports.CreateAuctionCtrl = CreateAuctionCtrl;


/***/ },
/* 3 */
/***/ function(module, exports) {

	"use strict";
	var CommandHandlingErrorType;
	(function (CommandHandlingErrorType) {
	    CommandHandlingErrorType[CommandHandlingErrorType["FailedToConnectToFeedbackHub"] = 0] = "FailedToConnectToFeedbackHub";
	    CommandHandlingErrorType[CommandHandlingErrorType["FailedToQueue"] = 1] = "FailedToQueue";
	    CommandHandlingErrorType[CommandHandlingErrorType["FeedbackTimeout"] = 2] = "FeedbackTimeout";
	    CommandHandlingErrorType[CommandHandlingErrorType["FailedToProcess"] = 3] = "FailedToProcess";
	})(CommandHandlingErrorType = exports.CommandHandlingErrorType || (exports.CommandHandlingErrorType = {}));


/***/ },
/* 4 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var CommandHandler_1 = __webpack_require__(5);
	var CancelAuctionCommandHandler = (function (_super) {
	    __extends(CancelAuctionCommandHandler, _super);
	    function CancelAuctionCommandHandler() {
	        return _super.apply(this, arguments) || this;
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
	        return _super.apply(this, arguments) || this;
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
	    'CancelAuctionCommandHandler': CancelAuctionCommandHandler,
	    'CreateAuctionCommandHandler': CreateAuctionCommandHandler,
	    'MakeBidCommandHandler': MakeBidCommandHandler,
	};
	exports.AngularCommandHandlersRegistry = AngularCommandHandlersRegistry;


/***/ },
/* 5 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var CommandHandlingErrorType_1 = __webpack_require__(3);
	var CommandHandler = (function () {
	    function CommandHandler(httpService, qService, timeoutService) {
	        this.httpService = httpService;
	        this.qService = qService;
	        this.timeoutService = timeoutService;
	        if (!CommandHandler.wasSignalrRInitialized) {
	            var commandHandlingFeedbackHub = $.connection.commandHandlingFeedbackHub;
	            commandHandlingFeedbackHub.client.handleCommandSuccess = function (commandHandlingSucceededEvent) {
	                CommandHandler.commandHandlingSuccessCallbacks.fire(commandHandlingSucceededEvent);
	            };
	            commandHandlingFeedbackHub.client.handleCommandFailure = function (commandHandlingFailedEvent) {
	                CommandHandler.commandHandlingFailureCallbacks.fire(commandHandlingFailedEvent);
	            };
	            CommandHandler.wasSignalrRInitialized = true;
	        }
	    }
	    CommandHandler.prototype.handle = function (command) {
	        var _this = this;
	        var url = "api/" + this.getCommandName() + "/Handle";
	        var deferred = this.qService.defer();
	        this.connectSignalR()
	            .then(function () {
	            _this.httpService.post(url, command)
	                .then(function () {
	                var wasPromiseResolvedOrRejected = false;
	                var commandHandlingSuccessCallback = function (commandHandlingSucceededEvent) {
	                    if (commandHandlingSucceededEvent.CommandId === command.id) {
	                        wasPromiseResolvedOrRejected = true;
	                        deferred.resolve();
	                    }
	                };
	                var commandHandlingFailureCallback = function (commandHandlingFailedEvent) {
	                    if (commandHandlingFailedEvent.CommandId === command.id) {
	                        wasPromiseResolvedOrRejected = true;
	                        deferred.reject(CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToProcess);
	                    }
	                };
	                CommandHandler.commandHandlingSuccessCallbacks.add(commandHandlingSuccessCallback);
	                CommandHandler.commandHandlingFailureCallbacks.add(commandHandlingFailureCallback);
	                var removeCallbacks = function () {
	                    CommandHandler.commandHandlingSuccessCallbacks.remove(commandHandlingSuccessCallback);
	                    CommandHandler.commandHandlingFailureCallbacks.remove(commandHandlingFailureCallback);
	                };
	                deferred.promise.finally(removeCallbacks);
	                var commandHandlingTimeoutMilliseconds = 4 * 1000;
	                _this.timeoutService(commandHandlingTimeoutMilliseconds)
	                    .then(function () {
	                    if (!wasPromiseResolvedOrRejected) {
	                        deferred.reject(CommandHandlingErrorType_1.CommandHandlingErrorType.FeedbackTimeout);
	                    }
	                });
	            })
	                .catch(function () { return deferred.reject(CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToQueue); });
	        })
	            .catch(function () { return deferred.reject(CommandHandlingErrorType_1.CommandHandlingErrorType.FailedToConnectToFeedbackHub); });
	        return deferred.promise;
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
	CommandHandler.$inject = ['$http', '$q', '$timeout'];
	exports.CommandHandler = CommandHandler;


/***/ }
/******/ ]);
//# sourceMappingURL=ApplicationBundle.js.map