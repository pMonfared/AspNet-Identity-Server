import { DataEventRecordsListComponent } from './dataeventrecords-list.component';
import { DataEventRecordsCreateComponent } from './dataeventrecords-create.component';
import { DataEventRecordsEditComponent } from './dataeventrecords-edit.component';
export var DATA_RECORDS_ROUTES = [
    {
        path: 'dataeventrecords',
        children: [
            { path: '', redirectTo: 'list', pathMatch: 'full' },
            {
                path: 'create',
                component: DataEventRecordsCreateComponent
            },
            {
                path: 'edit/:id',
                component: DataEventRecordsEditComponent
            },
            {
                path: 'list',
                component: DataEventRecordsListComponent,
            }
        ]
    }
];
//# sourceMappingURL=dataeventrecords.routes.js.map