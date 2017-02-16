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
import { Http, Headers } from '@angular/http';
import 'rxjs/add/operator/map';
import { Configuration } from '../app.constants';
import { SecurityService } from '../services/SecurityService';
var SecureFileService = (function () {
    function SecureFileService(_http, _configuration, _securityService) {
        var _this = this;
        this._http = _http;
        this._configuration = _configuration;
        this._securityService = _securityService;
        this.GetListOfFiles = function () {
            _this.setHeaders();
            return _this._http.get(_this.fileExplorerUrl, {
                headers: _this.headers,
                body: ''
            }).map(function (res) { return res.json(); });
        };
        this.actionUrl = _configuration.FileServer + "api/Download/";
        this.fileExplorerUrl = _configuration.FileServer + "api/FileExplorer/";
    }
    SecureFileService.prototype.DownloadFile = function (id) {
        var _this = this;
        this.setHeaders();
        var oneTimeAccessToken = '';
        this._http.get(this.actionUrl + "GenerateOneTimeAccessToken/" + id, {
            headers: this.headers,
            body: ''
        }).map(function (res) { return res.text(); }).subscribe(function (data) {
            oneTimeAccessToken = data;
        }, function (error) { return _this._securityService.HandleError(error); }, function () {
            console.log("open DownloadFile for file " + id + ": " + _this.actionUrl + oneTimeAccessToken);
            window.open("" + _this.actionUrl + oneTimeAccessToken);
        });
    };
    SecureFileService.prototype.setHeaders = function () {
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
        var token = this._securityService.GetToken();
        if (token !== '') {
            this.headers.append('Authorization', 'Bearer ' + token);
        }
    };
    return SecureFileService;
}());
SecureFileService = __decorate([
    Injectable(),
    __metadata("design:paramtypes", [Http, Configuration, SecurityService])
], SecureFileService);
export { SecureFileService };
//# sourceMappingURL=SecureFileService.js.map