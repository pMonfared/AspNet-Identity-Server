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
import { Router } from '@angular/router';
import { SecurityService } from '../services/SecurityService';
import { DataEventRecordsService } from '../dataeventrecords/DataEventRecordsService';
var DataEventRecordsCreateComponent = (function () {
    function DataEventRecordsCreateComponent(_dataEventRecordsService, securityService, _router) {
        this._dataEventRecordsService = _dataEventRecordsService;
        this.securityService = securityService;
        this._router = _router;
        this.message = 'DataEventRecords Create';
    }
    DataEventRecordsCreateComponent.prototype.ngOnInit = function () {
        this.DataEventRecord = { Id: 0, Name: '', Description: '' };
        console.log('IsAuthorized:' + this.securityService.IsAuthorized);
        console.log('HasAdminRole:' + this.securityService.HasAdminRole);
    };
    DataEventRecordsCreateComponent.prototype.Create = function () {
        var _this = this;
        this._dataEventRecordsService
            .Add(this.DataEventRecord)
            .subscribe(function (data) { return _this.DataEventRecord = data; }, function (error) { return _this.securityService.HandleError(error); }, function () { return _this._router.navigate(['/dataeventrecords']); });
    };
    return DataEventRecordsCreateComponent;
}());
DataEventRecordsCreateComponent = __decorate([
    Component({
        selector: 'dataeventrecords-create',
        templateUrl: 'dataeventrecords-create.component.html'
    }),
    __metadata("design:paramtypes", [DataEventRecordsService, SecurityService, Router])
], DataEventRecordsCreateComponent);
export { DataEventRecordsCreateComponent };
//# sourceMappingURL=dataeventrecords-create.component.js.map