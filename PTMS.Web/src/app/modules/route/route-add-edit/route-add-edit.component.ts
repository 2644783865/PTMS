import { Component, OnInit } from '@angular/core';
import { Observable, BehaviorSubject, combineLatest, of } from 'rxjs';
import { RouteFullDto, ProjectDto, BusStationDto, BusStationRouteDto } from '@app/core/dtos';
import { ActivatedRoute, Router, NavigationExtras } from '@angular/router';
import { RouteQuery } from '../route.state';
import { RouteService } from '../route.service';
import { FormGroup, Validators, FormArray, FormBuilder } from '@angular/forms';
import { switchMap } from 'rxjs/operators';
import { InlineFormHelper } from '@app/core/helpers';
import { KeyValuePair } from '@app/core/helpers';
import { CdkDragDrop } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-route-add-edit',
  templateUrl: './route-add-edit.component.html'
})
export class RouteAddEditComponent implements OnInit {
  private _stationSearchString = new BehaviorSubject<string>(null);
  private _inlineFormHelper: InlineFormHelper<BusStationRouteDto>;
  private _routeFull: RouteFullDto;

  projects$: Observable<ProjectDto[]>;
  busStations$: Observable<BusStationDto[]>;
  modalLoading$: Observable<boolean>;
  statuses: KeyValuePair<boolean>[];
  editForm: FormGroup;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private routeQuery: RouteQuery,
    private routeService: RouteService,
    private fb: FormBuilder) { }

  async ngOnInit() {
    this.busStations$ = combineLatest(
      this.routeQuery.busStations$, 
      this._stationSearchString)
      .pipe(switchMap(([busStations, searchString]) => {
        let result = [];

        if (searchString) {
          searchString = searchString.toLowerCase();
          result = busStations
            .filter(x => x.name.toLowerCase().includes(searchString))
            .slice(0, 20);
        }

        return of(result);
      }));
    
    this.projects$ = this.routeQuery.projects$;
    this.modalLoading$ = this.routeQuery.modalLoading$;

    this.statuses = [
      {key: "Активен", value: true},
      {key: "Не активен", value: false}
    ];

    let routeId = this.activatedRoute.snapshot.params.id;
    this._routeFull = await this.routeService.loadDataForEdit(routeId);
    this.mapToForm(this._routeFull);
  }

  get header(): string {
    if (this.activatedRoute.snapshot.params.id){
      
      if (this.editForm){
        return "Редактирование маршрута " + this.editForm.get('name').value;
      }
      else {
        return "Редактирование маршрута ";
      }
    }

    return "Добавление маршрута";
  }

  get isEditingInProgress(): boolean {
    return this._inlineFormHelper.isEditingInProgress;
  }
  
  isItemEditingInProgress(formGroup: FormGroup): boolean {
    return this._inlineFormHelper.isItemEditingInProgress(formGroup);
  }

  searchBusStation(stationSearchString: string){
    this._stationSearchString.next(stationSearchString);
  }

  addNewStation(index: number) {
    let num = index == 0 
      ? (this.editForm.controls.busStationRoutes as FormArray).length + 1
      : index + 1;

    let initialValue = { num };
    this._inlineFormHelper.addNew(initialValue, index);
  }

  editStation(formGroup: FormGroup) {
    this._inlineFormHelper.prepareOnEdit(formGroup);
  }

  saveStation(formGroup: FormGroup) {
    this._inlineFormHelper.prepareOnSave(formGroup);
    this.checkSortOrder(formGroup);  
    this._stationSearchString.next(null);
  }

  cancelStation(formGroup: FormGroup) {
    this._inlineFormHelper.validateOnCancel(formGroup);
  }

  deleteStation(formGroup: FormGroup){
    this._inlineFormHelper.delete(formGroup);
    this.checkSortOrder();
  }

  onDragDrop(event: CdkDragDrop<FormGroup[]>) {
    let formArray = this.editForm.controls.busStationRoutes as FormArray;
    let updatedFormGroup = formArray.controls[event.previousIndex] as FormGroup;
    updatedFormGroup.get('num').setValue(event.currentIndex + 1);
    this.checkSortOrder(updatedFormGroup);
  }

  async onSubmit() {
    let busStationRoutesResult = this._inlineFormHelper.getValuesToSave(this._routeFull.busStationRoutes);
    let saveResult = await this.routeService.save(this.editForm.value, busStationRoutesResult);
    
    if (saveResult.success) {
      if (this._routeFull.id) {
        await this.ngOnInit();
      }
      else {
        this.router.navigateByUrl(`routes/${saveResult.route.id}`);
      }
    }
    else if (saveResult.route) {
      this.editForm.get('id').setValue(saveResult.route.id);
      saveResult.busStationRoutes.forEach(x => this._inlineFormHelper.update(x));

      this._routeFull = await this.routeService.loadDataForEdit(saveResult.route.id);
    }
  }

  onClose() {
    this.router.navigateByUrl('routes');
  }

  ngOnDestroy() {
    this.routeService.onDestroy();
    this._stationSearchString.complete();
  }

  private checkSortOrder(updatedFormGroup: FormGroup = null) {
    let formArray = this.editForm.controls.busStationRoutes as FormArray;

    if (updatedFormGroup) {
      //update sort order on add/edit
      let oldSortOrder = formArray.controls.indexOf(updatedFormGroup) + 1;
      let newSortOrder = updatedFormGroup.get('num').value;

      if (oldSortOrder >= newSortOrder) {
        updatedFormGroup.get('num').setValue(newSortOrder-0.5);
      }
      else if (oldSortOrder < newSortOrder) {
        updatedFormGroup.get('num').setValue(newSortOrder+0.5);
      }

      //order by num
      let array = formArray.value;
      array.sort((a, b) => a.num - b.num)
      formArray.patchValue(array);
    }
    
    //update sort order - set equal to index
    formArray.controls.forEach((formGroup, index) => {
      let sortOrder = index+1;
      formGroup.get('num').setValue(sortOrder);
    });
  }

  private mapToForm(route: RouteFullDto){
    this.editForm = this.fb.group({
      id: [route.id],
      name: [route.name, Validators.required],
      projectId: [route.projectId],
      routeActive: [route.routeActive, Validators.required],
      busStationRoutes: this.fb.array([])
    });

    let formArray = this.editForm.controls.busStationRoutes as FormArray;

    this._inlineFormHelper = new InlineFormHelper<BusStationRouteDto>(
      formArray,
      this.getEmptyStationFormGroup.bind(this),
      this.mapToFormGroupValue.bind(this));

    this._inlineFormHelper.initForm(route.busStationRoutes.sort((a, b) => a.num - b.num));

    formArray.setValidators(this._inlineFormHelper.atLeastOneValidator);
  }

  private mapToFormGroupValue(bsRoute: BusStationRouteDto): Object {
    let busStation = this.routeQuery.getValue().busStations.find(b => b.id == bsRoute.busStationId);

    return {
      busStation,
      num: bsRoute.num,
      isEndingStation: bsRoute.isEndingStation
    };
  }

  private getEmptyStationFormGroup(): FormGroup{
    return this.fb.group({
      busStation: [null, Validators.required],
      num: [null, Validators.required],
      isEndingStation: [false]
    });
  }
}
