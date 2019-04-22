Ext.define('Core.form.editabledetail.modes.LoadedMode', {

    loadRecord: function (record) {
        if (record) {
            this.form.loadRecord(record);
        } else {
            this.form.reset(true);
            this.changeMode(Core.BaseEditableDetailForm.EMPTY_MODE);
        }
        this.setEditing(false);
    },

    getRecord: function () {
        return this.form.getRecord();
    },

    startEditRecord: function () {
        this.previousRecord = this.form.getRecord();
        this.changeMode(Core.BaseEditableDetailForm.EDITING_MODE);
        this.setEditing(true);
    },

    startCreateRecord: function (record) {
        this.previousRecord = this.form.getRecord();
        this.form.loadRecord(record || Ext.create(this.model));
        this.changeMode(Core.BaseEditableDetailForm.CREATING_MODE);
        this.setEditing(true);
    },

    commitChanges: function () {
        console.info('Nothing to commit');
    },

    rejectChanges: function () {
        console.info('Nothing to reject');
    }

});