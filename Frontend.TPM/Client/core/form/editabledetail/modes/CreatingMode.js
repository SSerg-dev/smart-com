Ext.define('Core.form.editabledetail.modes.CreatingMode', {

    loadRecord: function (record) {
        console.info('Can not load record while creating');
    },

    getRecord: function () {
        console.info('Can not get record while creating');
        return null;
    },

    startEditRecord: function () {
        console.info('Can not start editing record while creating');
    },

    startCreateRecord: function () {
        console.info('Already creating');
    },

    commitChanges: function () {
     
        this.changeMode(Core.BaseEditableDetailForm.CREATED_MODE);

        return (function (success) {
            if (success) {
                this.form.updateRecord();
                this.setEditing(false);
                this.changeMode(Core.BaseEditableDetailForm.LOADED_MODE);
            } else {
                this.changeMode(Core.BaseEditableDetailForm.CREATING_MODE);
                this.setEditing(true);
            }
        }).bind(this);
    },

    rejectChanges: function () {
        this.form.reset(true);
        if (this.previousRecord) {
            this.form.loadRecord(this.previousRecord);
        }
        this.setEditing(false);
        this.changeMode(Core.BaseEditableDetailForm.LOADED_MODE);
    }

});