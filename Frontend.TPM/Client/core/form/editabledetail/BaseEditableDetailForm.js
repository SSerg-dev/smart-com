Ext.define('Core.form.editabledetail.BaseEditableDetailForm', {
    extend: 'Core.form.ColumnLayoutForm',
    alternateClassName: 'Core.BaseEditableDetailForm',
    alias: 'widget.baseeditabledetailform',

    ui: 'detailform-panel',
    cls: 'detailpanel',
    overflowY: 'auto',

    statics: {
        EMPTY_MODE: 'empty',
        LOADED_MODE: 'loaded',
        CREATING_MODE: 'creating',
        EDITING_MODE: 'editing',
        CREATED_MODE: 'created',
        CHANGED_MODE: 'changed',

        modeInstances: {
            'empty': Ext.create('Core.form.editabledetail.modes.EmptyMode'),
            'loaded': Ext.create('Core.form.editabledetail.modes.LoadedMode'),
            'creating': Ext.create('Core.form.editabledetail.modes.CreatingMode'),
            'editing': Ext.create('Core.form.editabledetail.modes.EditingMode'),
            'created': Ext.create('Core.form.editabledetail.modes.CreatedMode'),
            'changed': Ext.create('Core.form.editabledetail.modes.ChangedMode')
        }
    },

    constructor: function () {
        this.itemDefaults = {
            editableModes: [
                Core.BaseEditableDetailForm.CREATING_MODE,
                Core.BaseEditableDetailForm.EDITING_MODE,
                Core.BaseEditableDetailForm.CREATED_MODE,
                Core.BaseEditableDetailForm.CHANGED_MODE
            ]
        };

        this.callParent(arguments);
        this.model = Ext.ModelManager.getModel(this.model);
        this.changeMode('empty');
    },

    initComponent: function () {
        this.addEvents(
            'modechange'
        );
        this.callParent(arguments);
        this.setEditing(false);
    },

    // TODO: вынести
    collapseSystemTabs: function () {
        var systemTab = this.up('viewport').down('system');
        if (systemTab.getActiveTab())
            systemTab.collapse();
    },

    loadRecord: function (record) {
        this.getModeInstance().loadRecord.call(this, record);
    },

    getRecord: function () {
        return this.getModeInstance().getRecord.call(this);
    },

    startEditRecord: function () {
        this.getModeInstance().startEditRecord.call(this);

        this.collapseSystemTabs();

        this.afterFormShow(this, false);
    },

    //For override in view
    afterFormShow: function (scope, isCreating) { },

    beforeCommitChanges: function (scope) { },
    beforeRejectChanges: function (scope) { },

    startCreateRecord: function (record) {
        this.setValidateOnChange(false);
        this.getModeInstance().startCreateRecord.call(this, record);
        this.setValidateOnChange(true);

        this.collapseSystemTabs();

        this.afterFormShow(this, true);
    },

    commitChanges: function () {
        this.beforeCommitChanges(this);
        return this.getModeInstance().commitChanges.call(this);
    },

    rejectChanges: function () {
        this.beforeRejectChanges(this);
        this.getModeInstance().rejectChanges.call(this);

        if (this.mode != Core.BaseEditableDetailForm.LOADED_MODE) {
            this.form.reset(true);
        }
    },

    isValid: function () {
        var form = this.form,
            record = this.getRecord(),
            errors;

        if (!form.isValid()) {
            return false;
        }

        errors = record.validate();

        if (!errors.isValid()) {
            form.markInvalid(errors);
            return false;
        }

        return true;
    },

    /**
     * private
     */
    changeMode: function (newMode) {
        var oldMode = this.mode;

        if (!newMode) {
            console.error('Argument \'newMode\' should not be null');
            return;
        }

        this.mode = newMode;
        this.fireEvent('modechange', this, newMode, oldMode);
        this.onModeChange(newMode, oldMode);
    },

    onModeChange: Ext.emptyFn,

    /**
     * private
     */
    getModeInstance: function () {
        return Core.BaseEditableDetailForm.modeInstances[this.mode] || null;
    },

    /**
     * private
     */
    setEditing: function (editing) {
        this.form.getFields().each(function (field) {
            if (editing && field.editableModes && !Ext.Array.contains(field.editableModes, this.mode)) {
                field.setReadOnly(true);
                field.addCls('readonly-field');
            } else {
                field.setReadOnly(!editing);
                // для нередактируемых полей (searchfield итп) после 2-го вызова setReadOnly(false) пропадает блокировка поля ввода
                if (editing && field.editable === false && field.inputEl.dom.readOnly === false) {
                    field.inputEl.dom.readOnly = true;
                }
                field.removeCls('readonly-field');
            }
        }, this);
    },

    setValidateOnChange: function (validateOnChange) {
        this.form.getFields().each(function (field) {
            field.validateOnChange = validateOnChange;
        }, this);
    }

});