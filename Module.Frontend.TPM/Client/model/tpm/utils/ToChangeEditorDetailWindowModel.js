Ext.define('App.model.tpm.utils.ToChangeEditorDetailWindowModel', {
    extend: 'Core.form.EditorDetailWindowModel',
    name: 'ToChangeEditorDetailWindowModel',

    startDetailRecord: function (model) {
        this.editor = this.createEditor({ title: l10n.ns('core').value('detailWindowTitle') });
        var isHistorical = this.editor.down('#historicaldetailform');
        var isDeleted = this.editor.down('#deleteddetailform');

        // если запись из гридов История или Удаленные, то скрывается кнопка Редактировать

        //Проверка на доступ к изменению данной модели для тоже скрытия
        var toEditAccess = false;
        if (model.proxy) {
            if (model.proxy.type == 'breeze') {
                toEditAccess = App.UserInfo.hasAccessPoint(model.proxy.resourceName, 'Patch');
            }
        }

        this.getForm().getForm().getFields().each(function (field, index, len) {
            field.setReadOnly(true);
        }, this);

        // Приводим editor к EditorDetailWindow.
        if (this.editor.isOriginalEditor === true) {
            this.editor.down('#ok').text = l10n.ns('core', 'buttons').value('ok');
            this.editor.down('#close').text = l10n.ns('core', 'buttons').value('close');

            var cancelEditButton = {
                xtype: 'button',
                itemId: 'canceledit',
                text: l10n.ns('core', 'buttons').value('cancel'),
                ui: "white-button-footer-toolbar"
            };

            var editButton = {
                xtype: 'button',
                itemId: 'edit',
                text: l10n.ns('core', 'buttons').value('edit'),
                ui: 'green-button-footer-toolbar'
            };

            this.editor.down('#ok').up().insert(1, cancelEditButton);
            this.editor.down('#ok').up().insert(2, editButton);
        }

        if (!isHistorical && !isDeleted && toEditAccess) {
            this.editor.down('#ok').setVisible(false);
            this.editor.down('#canceledit').setVisible(false);

            this.editor.down('#ok').on('click', this.onOkButtonClick, this);
            this.editor.down('#edit').on('click', this.onEditButtonClick, this);
            this.editor.down('#canceledit').on('click', this.onCancelButtonClick, this);
            this.editor.down('#close').on('click', this.onCloseButtonClick, this);
            this.editor.on('close', this.onEditorClose, this);
        } else {
            this.editor.down('#ok').setVisible(false);
            this.editor.down('#edit').setVisible(false);
            this.editor.down('#canceledit').setVisible(false);

            this.editor.down('#close').on('click', this.onCloseButtonClick, this);
            this.editor.on('close', this.onEditorClose, this);
        }

        this.getForm().loadRecord(model);
        this.editor.show();
    },

    // Privates. 
    createEditor: function (cfg) {
        var grid = this.getGrid();
        var tree = this.getTree(); // Возможен вызов из дерева
        var tmpModel = grid ? grid.getStore().model : tree.getStore().model;
        var model = Ext.ModelManager.getModel(tmpModel);
        var panel = grid ? grid.up('combineddirectorypanel') : tree.up('combineddirectorytreepanel');
        var editor = null;

        try {
            editor = Ext.create(App.Util.buildViewClassName(panel, model, null, 'Editor'), cfg);
            editor.isOriginalEditor = true;
        } catch (err) {
            var detailgrid = grid.up().down('editabledetailform') ? grid.up().down('editabledetailform') : grid.up().down('detailform');
            var itemsTo = detailgrid.initialConfig.items;
            cfg.extend = 'App.view.core.common.EditorDetailWindow';
            cfg.width = 800;
            cfg.minWidth = 800;
            cfg.maxHeight = 600;
            cfg.items = {
                xtype: 'editorform',
                items: itemsTo
            };

            editor = Ext.create('App.view.core.common.EditorDetailWindow', cfg);
        } finally {
            return editor;
        }
    }
});