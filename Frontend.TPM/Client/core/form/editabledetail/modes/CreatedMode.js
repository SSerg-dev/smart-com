Ext.define('Core.form.editabledetail.modes.CreatedMode', {

    loadRecord: function (record) {
        console.info('Can not load record while creating');
    },

    getRecord: function () {
        return this.form.getRecord();
    },

    startEditRecord: function () {
        console.info('Can not edit record while creating');
    },

    startCreateRecord: function () {
        console.info('Record already created');
    },

    commitChanges: function () {
        console.info('Changes already commited');
    },

    rejectChanges: function () {
        this.form.reset(true);
        this.form.loadRecord(this.previousRecord);
        this.setEditing(false);
        this.changeMode(Core.BaseEditableDetailForm.LOADED_MODE);
    }

});