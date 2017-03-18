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
	var SecurityService_1 = __webpack_require__(7);
	var SecurityUiService_1 = __webpack_require__(8);
	var SignInDialogComponent_1 = __webpack_require__(9);
	var ApplicationCtrl_1 = __webpack_require__(11);
	var Routing_1 = __webpack_require__(12);
	var GeneratedQueryHandlers_1 = __webpack_require__(13);
	var DisplayAuctionComponent_1 = __webpack_require__(15);
	var BusyIndicator_1 = __webpack_require__(17);
	var GeneratedUiCommandHandlers_1 = __webpack_require__(18);
	var AuctionsListComponent_1 = __webpack_require__(19);
	var BusyIndicatingHttpInterceptor_1 = __webpack_require__(21);
	var SimpleNotificationDialogComponent_1 = __webpack_require__(22);
	var GenericModalService_1 = __webpack_require__(25);
	var Configuration_1 = __webpack_require__(27);
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
	    new SimpleNotificationDialogComponent_1.SimpleNotificationDialogComponent()
	];
	exports.Application = Application;
	Application.bootstrap();


/***/ },
/* 1 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var CreateAuctionCtrl_1 = __webpack_require__(!(function webpackMissingModule() { var e = new Error("Cannot find module \"./CreateAuctionCtrl\""); e.code = 'MODULE_NOT_FOUND'; throw e; }()));
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
/* 2 */,
/* 3 */,
/* 4 */,
/* 5 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var CommandHandler_1 = __webpack_require__(!(function webpackMissingModule() { var e = new Error("Cannot find module \"./CommandHandler\""); e.code = 'MODULE_NOT_FOUND'; throw e; }()));
	var PopulateDatabaseWithTestDataCommandHandler = (function (_super) {
	    __extends(PopulateDatabaseWithTestDataCommandHandler, _super);
	    function PopulateDatabaseWithTestDataCommandHandler() {
	        return _super.apply(this, arguments) || this;
	    }
	    PopulateDatabaseWithTestDataCommandHandler.prototype.getCommandName = function () {
	        return 'PopulateDatabaseWithTestDataCommand';
	    };
	    return PopulateDatabaseWithTestDataCommandHandler;
	}(CommandHandler_1.CommandHandler));
	exports.PopulateDatabaseWithTestDataCommandHandler = PopulateDatabaseWithTestDataCommandHandler;
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
	    'populateDatabaseWithTestDataCommandHandler': PopulateDatabaseWithTestDataCommandHandler,
	    'createAuctionCommandHandler': CreateAuctionCommandHandler,
	    'finishAuctionCommandHandler': FinishAuctionCommandHandler,
	    'makeBidCommandHandler': MakeBidCommandHandler,
	};
	exports.AngularCommandHandlersRegistry = AngularCommandHandlersRegistry;


/***/ },
/* 6 */,
/* 7 */
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
/* 8 */
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
/* 9 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var SignInDialogCtrl_1 = __webpack_require__(10);
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
/* 10 */
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
/* 11 */
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
	    ApplicationCtrl.prototype.checkIfAuctionsSearchQueryStringIsValid = function () {
	        return _(this.auctionsSearchQueryString).isString() && !!this.auctionsSearchQueryString.trim();
	    };
	    ApplicationCtrl.prototype.searchAuctions = function () {
	        this.$state.go('auctionsSearch', { queryString: this.auctionsSearchQueryString });
	    };
	    return ApplicationCtrl;
	}());
	ApplicationCtrl.$inject = ['securityUiService', 'busyIndicator', '$state', '$timeout', '$scope'];
	exports.ApplicationCtrl = ApplicationCtrl;


/***/ },
/* 12 */
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
	                component: 'auctionsList'
	            },
	            {
	                name: 'auctionsSearch',
	                url: '/auction/search/{queryString}',
	                component: 'auctionsList',
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
/* 13 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var QueryHandler_1 = __webpack_require__(14);
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
	    'getAuctionDetailsQueryHandler': GetAuctionDetailsQueryHandler,
	    'searchAuctionsQueryHandler': SearchAuctionsQueryHandler,
	};
	exports.AngularQueryHandlersRegistry = AngularQueryHandlersRegistry;


/***/ },
/* 14 */
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
/* 15 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var DisplayAuctionCtrl_1 = __webpack_require__(!(function webpackMissingModule() { var e = new Error("Cannot find module \"./DisplayAuctionCtrl\""); e.code = 'MODULE_NOT_FOUND'; throw e; }()));
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
/* 16 */,
/* 17 */
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
/* 18 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var CommandUiHandler_1 = __webpack_require__(!(function webpackMissingModule() { var e = new Error("Cannot find module \"./CommandUiHandler\""); e.code = 'MODULE_NOT_FOUND'; throw e; }()));
	var PopulateDatabaseWithTestDataCommandUiHandler = (function (_super) {
	    __extends(PopulateDatabaseWithTestDataCommandUiHandler, _super);
	    function PopulateDatabaseWithTestDataCommandUiHandler(populateDatabaseWithTestDataCommandHandler, busyIndicator, securityUiService, qService, genericModalService) {
	        var _this = _super.call(this, busyIndicator, securityUiService, qService, genericModalService) || this;
	        _this.populateDatabaseWithTestDataCommandHandler = populateDatabaseWithTestDataCommandHandler;
	        return _this;
	    }
	    PopulateDatabaseWithTestDataCommandUiHandler.prototype.getCommandHandler = function () {
	        return this.populateDatabaseWithTestDataCommandHandler;
	    };
	    return PopulateDatabaseWithTestDataCommandUiHandler;
	}(CommandUiHandler_1.CommandUiHandler));
	PopulateDatabaseWithTestDataCommandUiHandler.$inject = ['populateDatabaseWithTestDataCommandHandler', 'busyIndicator', 'securityUiService', '$q', 'genericModalService'];
	exports.PopulateDatabaseWithTestDataCommandUiHandler = PopulateDatabaseWithTestDataCommandUiHandler;
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
	    'populateDatabaseWithTestDataCommandUiHandler': PopulateDatabaseWithTestDataCommandUiHandler,
	    'createAuctionCommandUiHandler': CreateAuctionCommandUiHandler,
	    'finishAuctionCommandUiHandler': FinishAuctionCommandUiHandler,
	    'makeBidCommandUiHandler': MakeBidCommandUiHandler,
	};
	exports.AngularCommandUiHandlersRegistry = AngularCommandUiHandlersRegistry;


/***/ },
/* 19 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var AuctionsListCtrl_1 = __webpack_require__(20);
	var AuctionsListComponent = (function () {
	    function AuctionsListComponent() {
	        this.controller = AuctionsListCtrl_1.AuctionsListCtrl;
	        this.templateUrl = 'Template/Auctions/List';
	        this.registerAs = 'auctionsList';
	        this.bindings = {
	            queryString: '<'
	        };
	    }
	    return AuctionsListComponent;
	}());
	exports.AuctionsListComponent = AuctionsListComponent;


/***/ },
/* 20 */
/***/ function(module, exports) {

	"use strict";
	var AuctionsListCtrl = (function () {
	    function AuctionsListCtrl(searchAuctionsQueryHandler) {
	        var _this = this;
	        this.searchAuctionsQueryHandler = searchAuctionsQueryHandler;
	        this.tastyInitCfg = {
	            'count': 10,
	            'page': 1
	        };
	        this.staticResource = {
	            header: [
	                {
	                    key: 'Title',
	                    name: 'Title',
	                    style: { width: '30%' }
	                },
	                {
	                    key: 'Description',
	                    name: 'Description',
	                    style: { width: '70%' }
	                }
	            ]
	        };
	        this.getResource = function (paramsString, paramsObject) {
	            var query = {
	                queryString: _this.queryString,
	                pageSize: paramsObject.count,
	                pageNumber: paramsObject.page
	            };
	            return _this.searchAuctionsQueryHandler.handle(query)
	                .then(function (auctionsList) {
	                return {
	                    rows: auctionsList.pageItems,
	                    pagination: {
	                        count: auctionsList.pageSize,
	                        page: auctionsList.pageNumber,
	                        pages: auctionsList.totalPagesCount,
	                        size: auctionsList.totalItemsCount
	                    },
	                    header: _this.staticResource.header
	                };
	            });
	        };
	    }
	    return AuctionsListCtrl;
	}());
	AuctionsListCtrl.$inject = ['searchAuctionsQueryHandler'];
	exports.AuctionsListCtrl = AuctionsListCtrl;


/***/ },
/* 21 */
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
/* 22 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var SimpleNotificationDialogCtrl_1 = __webpack_require__(23);
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
/* 23 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var NotificationType_1 = __webpack_require__(24);
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
/* 24 */
/***/ function(module, exports) {

	"use strict";
	var NotificationType;
	(function (NotificationType) {
	    NotificationType[NotificationType["Information"] = 0] = "Information";
	    NotificationType[NotificationType["Success"] = 1] = "Success";
	    NotificationType[NotificationType["Error"] = 2] = "Error";
	})(NotificationType = exports.NotificationType || (exports.NotificationType = {}));


/***/ },
/* 25 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var NotificationType_1 = __webpack_require__(24);
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
/* 26 */,
/* 27 */
/***/ function(module, exports) {

	"use strict";
	var Configuration = (function () {
	    function Configuration() {
	        this.commandHandlingTimeoutMilliseconds = 5 * 1000;
	        this.readModelChangeNotificationTimeoutMilliseconds = 5 * 1000;
	    }
	    return Configuration;
	}());
	Object.defineProperty(exports, "__esModule", { value: true });
	exports.default = Configuration;


/***/ }
/******/ ]);
//# sourceMappingURL=ApplicationBundle.js.map