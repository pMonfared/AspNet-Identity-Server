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
import { SecureFileService } from './SecureFileService';
import { SecurityService } from '../services/SecurityService';
var SecureFilesComponent = (function () {
    function SecureFilesComponent(_secureFileService, securityService) {
        this._secureFileService = _secureFileService;
        this.securityService = securityService;
        this.message = 'Secure Files download';
    }
    SecureFilesComponent.prototype.ngOnInit = function () {
        this.getData();
    };
    SecureFilesComponent.prototype.DownloadFileById = function (id) {
        this._secureFileService.DownloadFile(id);
    };
    SecureFilesComponent.prototype.getData = function () {
        var _this = this;
        this._secureFileService.GetListOfFiles()
            .subscribe(function (data) { return _this.Files = data; }, function (error) { return _this.securityService.HandleError(error); }, function () { return console.log('Get all completed'); });
    };
    return SecureFilesComponent;
}());
SecureFilesComponent = __decorate([
    Component({
        selector: 'securefiles',
        templateUrl: 'securefiles.component.html',
        providers: [SecureFileService]
    }),
    __metadata("design:paramtypes", [SecureFileService, SecurityService])
], SecureFilesComponent);
export { SecureFilesComponent };
//# sourceMappingURL=securefiles.component.js.map