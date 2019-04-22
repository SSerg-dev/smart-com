Ext.define('Core.form.EditorFormModel', {
    extend: 'Core.form.BaseEditorModel',
    firstCreateOpen: true,
    firstEditOpen: true,

    startCreateRecord: function (model) {
        var form = this.getForm(),
            panel = form.up('combineddirectorypanel');

        this.addHandlers(form.down('#createtoolbar'));

        panel.lastActiveItemId = panel.getLayout().getActiveItem().getItemId();
        this.switchToForm();
        form.startCreateRecord(model);
    },

    startEditRecord: function (model) {
        var form = this.getForm(),
            panel = form.up('combineddirectorypanel');

        this.addHandlers(form.down('#edittoolbar'));

        panel.lastActiveItemId = panel.getLayout().getActiveItem().getItemId();
        this.switchToForm();
        form.loadRecord(model);
        form.startEditRecord();
    },

    onOkButtonClick: function (button) {
        var form = this.getForm(),
            grid = this.getGrid(),
            confirmFn = form.commitChanges(),
            record = form.getRecord();

        if (!form.isValid()) {
            confirmFn(false);
            return;
        }

        //обновляем запись тут а не в "Mode"
        form.form.updateRecord();
        if (record.afterUpdate) {
            record.afterUpdate(grid);
        }

        console.log('record data: ', record.getData());
        this.saveModel(record, confirmFn);
        //this.switchToLastActiveItem();
    },

    onCancelButtonClick: function (button) {
        this.getForm().rejectChanges();
        this.switchToLastActiveItem();
    },

    getForm: function () {
        return this.getGrid().up('combineddirectorypanel').down('#detailform');
    },

    saveModel: function (model, callback) {
        var isCreate = model.phantom,
            grid = this.getGrid(),
            panel = grid.up('combineddirectorypanel'),
            store = grid.getStore();
        panel.setLoading(l10n.ns('core').value('savingText'));

        model.save({
            scope: this,
            success: function (rec, resp, opts) {
                if (callback) {
                    callback(true);
                }

                grid.setLoadMaskDisabled(true);

                store.on({
                    single: true,
                    scope: this,
                    load: function (records, operation, success) {
                        model.set('Key');
                        var activeView = panel.getLayout().getActiveItem();
                        var ind;
                        if (activeView.getItemId() === 'detailform') {
                            var formRecord = activeView.getRecord();

                            if (formRecord && formRecord.getId() === model.getId()) {
                                if (formRecord.afterUpdate) {
                                    formRecord.afterUpdate(grid);
                                }
                                formRecord.set('Key');
                                activeView.loadRecord(model);
                                if (isCreate) {
                                    ind = store.indexOfId(model.getId());
                                    // если созданная запись не попала в пейдж, развыделяем всё - блокируем кнопки
                                    // TODO: сохранять созданную или последнюю выделенную запись и раздизэйбливать кнопки если при прокрутке она попала в текущий page
                                    if (!ind || ind == -1) {
                                        grid.getSelectionModel().deselectAll();
                                    } else {
                                        grid.getSelectionModel().select(model);
                                    }
                                }
                            }
                        }

                        panel.setLoading(false);
                        grid.setLoadMaskDisabled(false);

                        if (typeof grid.afterSaveCallback === 'function') {
                            grid.afterSaveCallback(grid);
                        }

                        this.switchToLastActiveItem();
                        if (ind && ind != -1) {
                            grid.getView().bufferedRenderer.scrollTo(ind, true, function () {
                                grid.setLoadMaskDisabled(false);
                            });
                        }
                    }
                });

                store.load();
            },
            failure: function () {
                if (callback) {
                    callback(false);
                }
                //model.reject();
                panel.setLoading(false);
            }
        });
    },

    // Privates. 

    getCDPanel: function () {
        return this.getGrid().up('combineddirectorypanel');
    },

    switchToForm: function () {
        var layout = this.getCDPanel().getLayout();

        if (layout.type === 'card') {
            layout.setActiveItem('detailform');
        }
    },

    switchToLastActiveItem: function () {
        var panel = this.getCDPanel(),
            layout = panel.getLayout();

        if (panel.lastActiveItemId && layout.getActiveItem() !== panel.lastActiveItemId) {
            layout.setActiveItem(panel.lastActiveItemId);
        }
    },

    addHandlers: function (toolbar) {
        var isCreate = toolbar.itemId == 'createtoolbar';
        firstOpen = isCreate ? this.firstCreateOpen : this.firstEditOpen;
        if (firstOpen && toolbar) {
            toolbar.down('#ok').on({
                scope: this,
                click: this.onOkButtonClick
            });

            toolbar.down('#cancel').on({
                scope: this,
                click: this.onCancelButtonClick
            });
            isCreate ? this.firstCreateOpen = false : this.firstEditOpen = false;
        }
    }
});