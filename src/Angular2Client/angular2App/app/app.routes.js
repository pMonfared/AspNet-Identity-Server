import { RouterModule } from '@angular/router';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { HomeComponent } from './home/home.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { SecureFilesComponent } from './securefile/securefiles.component';
import { DATA_RECORDS_ROUTES } from './dataeventrecords/dataeventrecords.routes';
var appRoutes = [
    { path: '', component: HomeComponent },
    { path: 'home', component: HomeComponent },
    { path: 'Forbidden', component: ForbiddenComponent },
    { path: 'Unauthorized', component: UnauthorizedComponent },
    { path: 'securefile/securefiles', component: SecureFilesComponent }
].concat(DATA_RECORDS_ROUTES);
export var routing = RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routes.js.map