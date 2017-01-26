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
	var SecurityService_1 = __webpack_require__(6);
	var SecurityUiService_1 = __webpack_require__(7);
	var LoginDialogComponent_1 = __webpack_require__(8);
	var ApplicationCtrl_1 = __webpack_require__(10);
	var Routing_1 = __webpack_require__(11);
	var GeneratedQueryHandlers_1 = __webpack_require__(12);
	var DisplayAuctionComponent_1 = __webpack_require__(14);
	var Application = (function () {
	    function Application() {
	    }
	    Application.bootstrap = function () {
	        var module = angular.module('auctionHouse', [
	            'ui.router', 'formly', 'formlyBootstrap', 'ngMessages', 'ngAnimate', 'ui.bootstrap',
	            'ui.bootstrap.datetimepicker'
	        ]);
	        module.controller('applicationCtrl', ApplicationCtrl_1.ApplicationCtrl);
	        this.registerSerivces(module);
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
	    Application.registerSerivces = function (module) {
	        module.service(GeneratedCommandHandlers_1.AngularCommandHandlersRegistry.commandHandlers);
	        module.service(GeneratedQueryHandlers_1.AngularQueryHandlersRegistry.queryHandlers);
	        module.service('securityService', SecurityService_1.SecurityService);
	        module.service('securityUiService', SecurityUiService_1.SecurityUiService);
	    };
	    Application.configureModule = function ($stateProvider) {
	        Routing_1.Routing.configure($stateProvider);
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
	        formlyConfig.extras.errorExistsAndShouldBeVisibleExpression = 'fc.$touched || form.$submitted';
	    };
	    ;
	    return Application;
	}());
	Application.components = [
	    new CreateAuctionComponent_1.CreateAuctionComponent(),
	    new DisplayAuctionComponent_1.DisplayAuctionComponent(),
	    new LoginDialogComponent_1.LoginDialogComponent()
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
	    function CreateAuctionCtrl(createAuctionCommandHandler, getAuctionDetailsQueryHandler, stateService) {
	        this.createAuctionCommandHandler = createAuctionCommandHandler;
	        this.getAuctionDetailsQueryHandler = getAuctionDetailsQueryHandler;
	        this.stateService = stateService;
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
	        var _this = this;
	        if (!this.form.$valid) {
	            return;
	        }
	        this.createAuctionCommandHandler
	            .handle(this.model)
	            .then(function () {
	            alert('Success');
	            _this.stateService.go('displayAuction', { auctionId: _this.model.auctionId });
	        })
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
	CreateAuctionCtrl.$inject = ['createAuctionCommandHandler', 'getAuctionDetailsQueryHandler', '$state'];
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
	var CommandHandler_1 = __webpack_require__(!(function webpackMissingModule() { var e = new Error("Cannot find module \"./CommandHandler\""); e.code = 'MODULE_NOT_FOUND'; throw e; }()));
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
	    'cancelAuctionCommandHandler': CancelAuctionCommandHandler,
	    'createAuctionCommandHandler': CreateAuctionCommandHandler,
	    'makeBidCommandHandler': MakeBidCommandHandler,
	};
	exports.AngularCommandHandlersRegistry = AngularCommandHandlersRegistry;


/***/ },
/* 5 */,
/* 6 */
/***/ function(module, exports) {

	"use strict";
	var SecurityService = (function () {
	    function SecurityService(httpService) {
	        this.httpService = httpService;
	        this.currentUserName = null;
	    }
	    SecurityService.prototype.logIn = function (userName, password) {
	        var _this = this;
	        var loginCommand = {
	            userName: userName,
	            password: password
	        };
	        return this.httpService.post('api/Authentication/LogIn', loginCommand)
	            .then(function () {
	            _this.currentUserName = userName;
	        });
	    };
	    SecurityService.prototype.logOut = function () {
	        var _this = this;
	        if (!this.checkIfUserIsAuthenticated()) {
	            throw new Error('Current user is not authenticated.');
	        }
	        return this.httpService.post('api/Authentication/LogOut', {})
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
/* 7 */
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
	        return this.showLogInDialog();
	    };
	    SecurityUiService.prototype.showLogInDialog = function () {
	        var modalInstance = this.modalService.open({
	            component: 'loginDialog'
	        });
	        return modalInstance.result;
	    };
	    SecurityUiService.prototype.logOut = function () {
	        return this.securityService.logOut();
	    };
	    return SecurityUiService;
	}());
	SecurityUiService.$inject = ['securityService', '$uibModal', '$q'];
	exports.SecurityUiService = SecurityUiService;


/***/ },
/* 8 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var LoginDialogCtrl_1 = __webpack_require__(9);
	var LoginDialogComponent = (function () {
	    function LoginDialogComponent() {
	        this.controller = LoginDialogCtrl_1.LoginDialogCtrl;
	        this.templateUrl = 'Template/Security/LoginDialog';
	        this.registerAs = 'loginDialog';
	        this.bindings = {
	            modalInstance: '<'
	        };
	    }
	    return LoginDialogComponent;
	}());
	exports.LoginDialogComponent = LoginDialogComponent;


/***/ },
/* 9 */
/***/ function(module, exports) {

	"use strict";
	var LoginDialogCtrl = (function () {
	    function LoginDialogCtrl(securityService) {
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
	    LoginDialogCtrl.prototype.login = function () {
	        var _this = this;
	        if (!this.form.$valid) {
	            return;
	        }
	        this.securityService
	            .logIn(this.model.userName, this.model.password)
	            .then(function () {
	            _this.modalInstance.close();
	        }, function () {
	            // TODO: create generic notification dialogs
	            alert('error');
	        });
	    };
	    LoginDialogCtrl.prototype.cancel = function () {
	        this.modalInstance.dismiss();
	    };
	    return LoginDialogCtrl;
	}());
	LoginDialogCtrl.$inject = ['securityService'];
	exports.LoginDialogCtrl = LoginDialogCtrl;


/***/ },
/* 10 */
/***/ function(module, exports) {

	"use strict";
	var ApplicationCtrl = (function () {
	    function ApplicationCtrl(securityUiService) {
	        this.securityUiService = securityUiService;
	    }
	    return ApplicationCtrl;
	}());
	ApplicationCtrl.$inject = ['securityUiService'];
	exports.ApplicationCtrl = ApplicationCtrl;


/***/ },
/* 11 */
/***/ function(module, exports) {

	"use strict";
	var Routing = (function () {
	    function Routing() {
	    }
	    Routing.configure = function ($stateProvider) {
	        var states = [
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
	                    auctionId: function ($stateParams) {
	                        return $stateParams.auctionId;
	                    }
	                }
	            }
	        ];
	        for (var _i = 0, states_1 = states; _i < states_1.length; _i++) {
	            var state = states_1[_i];
	            $stateProvider.state(state);
	        }
	    };
	    return Routing;
	}());
	exports.Routing = Routing;


/***/ },
/* 12 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var QueryHandler_1 = __webpack_require__(13);
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
	var GetAuctionListQueryHandler = (function (_super) {
	    __extends(GetAuctionListQueryHandler, _super);
	    function GetAuctionListQueryHandler() {
	        return _super.apply(this, arguments) || this;
	    }
	    GetAuctionListQueryHandler.prototype.getQueryName = function () {
	        return 'GetAuctionListQuery';
	    };
	    return GetAuctionListQueryHandler;
	}(QueryHandler_1.QueryHandler));
	exports.GetAuctionListQueryHandler = GetAuctionListQueryHandler;
	var AngularQueryHandlersRegistry = (function () {
	    function AngularQueryHandlersRegistry() {
	    }
	    return AngularQueryHandlersRegistry;
	}());
	AngularQueryHandlersRegistry.queryHandlers = {
	    'getAuctionDetailsQueryHandler': GetAuctionDetailsQueryHandler,
	    'getAuctionListQueryHandler': GetAuctionListQueryHandler,
	};
	exports.AngularQueryHandlersRegistry = AngularQueryHandlersRegistry;


/***/ },
/* 13 */
/***/ function(module, exports) {

	"use strict";
	var QueryHandler = (function () {
	    function QueryHandler(httpService, qService, pushNotificationsService) {
	        this.httpService = httpService;
	        this.qService = qService;
	        this.pushNotificationsService = pushNotificationsService;
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
/* 14 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var DisplayAuctionCtrl_1 = __webpack_require__(15);
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
/* 15 */
/***/ function(module, exports) {

	"use strict";
	var DisplayAuctionCtrl = (function () {
	    function DisplayAuctionCtrl(getAuctionDetailsQueryHandler) {
	        var _this = this;
	        getAuctionDetailsQueryHandler.handle({
	            id: this.auctionId
	        })
	            .then(function (auction) { return _this.auction = auction; });
	    }
	    return DisplayAuctionCtrl;
	}());
	DisplayAuctionCtrl.$inject = ['getAuctionDetailsQueryHandler'];
	exports.DisplayAuctionCtrl = DisplayAuctionCtrl;


/***/ }
/******/ ]);
//# sourceMappingURL=ApplicationBundle.js.map