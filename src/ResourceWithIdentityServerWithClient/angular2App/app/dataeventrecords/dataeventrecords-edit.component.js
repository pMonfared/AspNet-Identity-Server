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
import { Router, ActivatedRoute } from '@angular/router';
import { SecurityService } from '../services/SecurityService';
import { DataEventRecordsService } from '../dataeventrecords/DataEventRecordsService';
var DataEventRecordsEditComponent = (function () {
    function DataEventRecordsEditComponent(_dataEventRecordsService, securityService, _route, _router) {
        this._dataEventRecordsService = _dataEventRecordsService;
        this.securityService = securityService;
        this._route = _route;
        this._router = _router;
        this.message = 'DataEventRecords Edit';
    }
    DataEventRecordsEditComponent.prototype.ngOnInit = function () {
        var _this = this;
        console.log('IsAuthorized:' + this.securityService.IsAuthorized);
        console.log('HasAdminRole:' + this.securityService.HasAdminRole);
        this.sub = this._route.params.subscribe(function (params) {
            var id = +params['id'];
            if (!_this.DataEventRecord) {
                _this._dataEventRecordsService.GetById(id)
                    .subscribe(function (data) { return _this.DataEventRecord = data; }, function (error) { return _this.securityService.HandleError(error); }, function () { return console.log('DataEventRecordsEditComponent:Get by Id complete'); });
            }
        });
    };
    DataEventRecordsEditComponent.prototype.ngOnDestroy = function () {
        this.sub.unsubscribe();
    };
    DataEventRecordsEditComponent.prototype.Update = function () {
        var _this = this;
        this._dataEventRecordsService.Update(this.id, this.DataEventRecord)
            .subscribe((function () { return console.log('subscribed'); }), function (error) { return _this.securityService.HandleError(error); }, function () { return _this._router.navigate(['/dataeventrecords']); });
    };
    return DataEventRecordsEditComponent;
}());
DataEventRecordsEditComponent = __decorate([
    Component({
        selector: 'dataeventrecords-edit',
        templateUrl: 'dataeventrecords-edit.component.html'
    }),
    __metadata("design:paramtypes", [DataEventRecordsService,
        SecurityService,
        ActivatedRoute,
        Router])
], DataEventRecordsEditComponent);
export { DataEventRecordsEditComponent };
//# sourceMappingURL=dataeventrecords-edit.component.js.map