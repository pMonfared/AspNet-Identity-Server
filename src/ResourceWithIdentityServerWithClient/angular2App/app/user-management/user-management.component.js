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
import { SecurityService } from '../services/SecurityService';
import { Router } from '@angular/router';
import { UserManagementService } from '../user-management/UserManagementService';
var UserManagementComponent = (function () {
    function UserManagementComponent(_userManagementService, securityService, _router) {
        this._userManagementService = _userManagementService;
        this.securityService = securityService;
        this._router = _router;
        this.message = 'user-management';
    }
    UserManagementComponent.prototype.ngOnInit = function () {
        this.getData();
    };
    UserManagementComponent.prototype.getData = function () {
        var _this = this;
        console.log('User Management:getData starting...');
        this._userManagementService
            .GetAll()
            .subscribe(function (data) { return _this.Users = data; }, function (error) { return _this.securityService.HandleError(error); }, function () { return console.log('User Management Get all completed'); });
    };
    UserManagementComponent.prototype.Update = function (user) {
        var _this = this;
        this._userManagementService.Update(user.id, user)
            .subscribe((function () { return console.log('subscribed'); }), function (error) { return _this.securityService.HandleError(error); }, function () { return console.log('update request sent!'); });
    };
    return UserManagementComponent;
}());
UserManagementComponent = __decorate([
    Component({
        selector: 'user-management',
        templateUrl: 'user-management.component.html'
    }),
    __metadata("design:paramtypes", [UserManagementService,
        SecurityService,
        Router])
], UserManagementComponent);
export { UserManagementComponent };
//# sourceMappingURL=user-management.component.js.map