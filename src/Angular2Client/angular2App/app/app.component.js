var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { SecurityService } from './services/SecurityService';
import './app.component.css';
var AppComponent = (function () {
    function AppComponent(securityService) {
        this.securityService = securityService;
    }
    AppComponent.prototype.ngOnInit = function () {
        console.log('ngOnInit _securityService.AuthorizedCallback');
        if (window.location.hash) {
            this.securityService.AuthorizedCallback();
        }
    };
    AppComponent.prototype.Login = function () {
        console.log('Do login logic');
        this.securityService.Authorize();
    };
    AppComponent.prototype.Logout = function () {
        console.log('Do logout logic');
        this.securityService.Logoff();
    };
    return AppComponent;
}());
AppComponent = __decorate([
    Component({
        selector: 'my-app',
        templateUrl: 'app.component.html'
    }),
    __metadata("design:paramtypes", [SecurityService])
], AppComponent);
export { AppComponent };
//# sourceMappingURL=app.component.js.map