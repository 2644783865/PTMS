import { FormArray, FormGroup, FormControl, ValidatorFn } from '@angular/forms';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

export interface InlineFormResult {
    toDelete: any[]
    toAdd: any[]
    toUpdate: any[]    
}

export class InlineFormHelper<T> {
    private _isEditingInProgress = false;
    private _previousFormGroupValue = null;
    private _isEditKey = "_isEdit";
    private _isAddKey = "_isAdd";

    constructor(
        private formArray: FormArray,
        private getEmptyFormGroup: () => FormGroup,
        private mapToFormGroupValue: (entity: T) => Object,
        private idKey: string = 'id'
    ) {

    }

    isItemEditingInProgress(formGroup: FormGroup): boolean {
        return formGroup.get([this._isEditKey]).value;
    }

    get isEditingInProgress(): boolean {
        return this._isEditingInProgress;
    }

    addNew(initalFormGroupValue: Object = null) {
        let formGroup = this.getEmptyFormGroupWithMarkers();

        if (initalFormGroupValue) {
            formGroup.patchValue(initalFormGroupValue);
        }

        formGroup.get(this._isEditKey).setValue(true);

        this.formArray.insert(0, formGroup);
        
        this._isEditingInProgress = true;
    }

    update(entity: T) {
        let formGroup = this.formArray.controls.find(formGroup => {
            return formGroup.get(this.idKey).value == entity[this.idKey];
        })

        let newValue = this.mapToFormGroupValueWithId(entity);
        formGroup.patchValue(newValue);
    }

    delete(formGroup: FormGroup) {
        let index = this.formArray.controls.indexOf(formGroup);
            
        if (index > -1){
            this.formArray.removeAt(index);
        }
        else {
            throw new Error("FormGroup is not found in FormArray")
        }
    }

    prepareOnEdit(formGroup: FormGroup){
        this._previousFormGroupValue = formGroup.value;
        formGroup.get(this._isEditKey).setValue(true);

        this._isEditingInProgress = true;
    }

    prepareOnSave(formGroup: FormGroup): Object {
        formGroup.get(this._isEditKey).setValue(false);
        formGroup.get(this._isAddKey).setValue(true);
        
        this._isEditingInProgress = false;

        let value = this.stripMarkers(formGroup.value);
        return value;
    }

    validateOnCancel(formGroup: FormGroup) {
        if (!formGroup.get(this._isAddKey).value) {
            this.delete(formGroup);
        }
        else {
            formGroup.reset(this._previousFormGroupValue);
            formGroup.get(this._isEditKey).setValue(false);
            this._previousFormGroupValue = null;
        }

        this._isEditingInProgress = false;
    }

    initForm(entities: T[]) {
        entities.forEach(entity => {
            let formGroup = this.getEmptyFormGroupWithMarkers();
            let value = this.mapToFormGroupValueWithId(entity);
            value[this._isAddKey] = true;
            value[this._isEditKey] = false;
    
            formGroup.setValue(value);
            this.formArray.push(formGroup);
        });
    }

    getValuesToSave(entities: T[]): InlineFormResult{
        let formGroupsValues = this.formArray.controls.map(x => x.value);

        let toAdd = [];
        let toUpdate = [];
        let toDelete = [];

        entities.forEach(entity => {
            let formGroupValue = formGroupsValues.find(fValue => {
                return fValue[this.idKey] == entity[this.idKey];
            });

            if (formGroupValue){
                this.stripMarkers(formGroupValue)
                if (this.isValueUpdated(formGroupValue, entity)){
                    toUpdate.push(formGroupValue);
                }                
            }
            else {
                toDelete.push(this.mapToFormGroupValueWithId(entity));
            }
        })

        toAdd = formGroupsValues.filter(fValue => {
            return !fValue[this.idKey];
        })

        let result = {
            toAdd,
            toUpdate,
            toDelete
        } as InlineFormResult

        return result;
    }

    get atLeastOneValidator(): ValidatorFn {
        let result = () => {
            let isValid = this.formArray.controls.length > 0;
            return !isValid ? {inlineFormAtLeastOne: true} : null;
        }

        return result.bind(this);
    }

    private mapToFormGroupValueWithId(entity: T): Object {
        let value = this.mapToFormGroupValue(entity);
        value[this.idKey] = entity[this.idKey];
        return value;
    }

    private getEmptyFormGroupWithMarkers() {
        let formGroup = this.getEmptyFormGroup();

        if (!formGroup.controls[this.idKey]){
            formGroup.addControl(this.idKey, new FormControl());
        }

        formGroup.addControl(this._isEditKey, new FormControl(false));
        formGroup.addControl(this._isAddKey, new FormControl(false));

        return formGroup;
    }

    private stripMarkers(value: Object): Object {
        delete value[this._isEditKey];
        delete value[this._isAddKey];

        return value;
    }

    private isValueUpdated(newValue: Object, entity: T): boolean {
        let oldValue = this.mapToFormGroupValueWithId(entity);

        for (let p in newValue) {
            if (oldValue[p] != newValue[p]) {
                return true;
            }
        }
    
        return false;
    }
}
