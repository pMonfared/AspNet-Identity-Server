var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import { Configuration } from '../app.constants';
import { SecurityService } from '../services/SecurityService';
var DataEventRecordsService = (function () {
    function DataEventRecordsService(_http, _configuration, _securityService) {
        var _this = this;
        this._http = _http;
        this._configuration = _configuration;
        this._securityService = _securityService;
        this.GetAll = function () {
            _this.setHeaders();
            var options = new RequestOptions({ headers: _this.headers, body: '' });
            return _this._http.get(_this.actionUrl, options).map(function (res) { return res.json(); });
        };
        this.GetById = function (id) {
            _this.setHeaders();
            return _this._http.get(_this.actionUrl + id, {
                headers: _this.headers,
                body: ''
            }).map(function (res) { return res.json(); });
        };
        this.Add = function (itemToAdd) {
            _this.setHeaders();
            return _this._http.post(_this.actionUrl, JSON.stringify(itemToAdd), { headers: _this.headers });
        };
        this.Update = function (id, itemToUpdate) {
            _this.setHeaders();
            return _this._http
                .put(_this.actionUrl + id, JSON.stringify(itemToUpdate), { headers: _this.headers });
        };
        this.Delete = function (id) {
            _this.setHeaders();
            return _this._http.delete(_this.actionUrl + id, {
                headers: _this.headers
            });
        };
        this.actionUrl = _configuration.Server + "/api/DataEventRecords/";
    }
    DataEventRecordsService.prototype.setHeaders = function () {
        console.log('setHeaders started');
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
        var token = this._securityService.GetToken();
        if (token !== '') {
            var tokenValue = 'Bearer ' + token;
            console.log('tokenValue:' + tokenValue);
            this.headers.append('Authorization', tokenValue);
        }
    };
    return DataEventRecordsService;
}());
DataEventRecordsService = __decorate([
    Injectable(),
    __metadata("design:paramtypes", [Http, Configuration, SecurityService])
], DataEventRecordsService);
export { DataEventRecordsService };
//# sourceMappingURL=DataEventRecordsService.js.map