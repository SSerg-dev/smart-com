Ext.define('App.util.TPM.CustomCharts.MinHeightColumn', {
    extend: Ext.chart.series.Column,
    alias: 'series.minheightcolumn',

    getPaths: function () {
        var me = this,
            chart = me.chart,
            store = chart.getChartStore(),
            data = store.data.items,
            i, total, record,
            bounds = me.bounds = me.getBounds(),
            items = me.items = [],
            yFields = Ext.isArray(me.yField) ? me.yField : [me.yField],
            gutter = me.gutter / 100,
            groupGutter = me.groupGutter / 100,
            animate = chart.animate,
            column = me.column,
            group = me.group,
            enableShadows = chart.shadow,
            shadowGroups = me.shadowGroups,
            shadowAttributes = me.shadowAttributes,
            shadowGroupsLn = shadowGroups.length,
            bbox = bounds.bbox,
            barWidth = bounds.barWidth,
            shrunkBarWidth = bounds.shrunkBarWidth,
            padding = me.getPadding(),
            stacked = me.stacked,
            barsLen = bounds.barsLen,
            colors = me.colorArrayStyle,
            colorLength = colors && colors.length || 0,
            themeIndex = me.themeIdx,
            reverse = me.reverse,
            math = Math,
            mmax = math.max,
            mmin = math.min,
            mabs = math.abs,
            minHeight = me.chart.minColumnHeight || 0,
            j, yValue, height, totalDim, totalNegDim, bottom, top, hasShadow, barAttr, attrs, counter,
            totalPositiveValues, totalNegativeValues,
            shadowIndex, shadow, sprite, offset, floorY, idx, itemIdx, xPos, yPos, width, usedWidth, barIdx;

        for (i = 0, total = data.length; i < total; i++) {
            record = data[i];
            bottom = bounds.zero;
            top = bounds.zero;
            totalDim = 0;
            totalNegDim = 0;
            totalPositiveValues = totalNegativeValues = 0;
            hasShadow = false;
            usedWidth = 0;

            for (j = 0, counter = 0; j < barsLen; j++) {

                if (me.__excludes && me.__excludes[j]) {
                    continue;
                }
                yValue = record.get(bounds.bars[j]);
                if (yValue >= 0) {
                    totalPositiveValues += yValue;
                } else {
                    totalNegativeValues += yValue;
                }

                height = Math.round((yValue - mmax(bounds.minY, 0)) * bounds.scale);

                //Единственное изменение
                if (height < minHeight && yValue != 0) {
                    height = minHeight;
                }

                idx = themeIndex + (barsLen > 1 ? j : 0);
                barAttr = {
                    fill: colors[idx % colorLength]
                };

                if (column) {
                    idx = reverse ? (total - i - 1) : i;
                    barIdx = reverse ? (barsLen - counter - 1) : counter;

                    if (me.boundColumn) {
                        xPos = bounds.barsLoc[idx];
                    }
                    else if (me.configuredColumnGirth && bounds.barsLoc.length) {
                        xPos = bounds.barsLoc[idx] +
                            barIdx * bounds.groupBarWidth *
                            (1 + groupGutter) * !stacked;

                    }
                    else {
                        xPos = bbox.x + padding.left +
                            (barWidth - shrunkBarWidth) * 0.5 +
                            idx * barWidth * (1 + gutter) +
                            barIdx * bounds.groupBarWidth *
                            (1 + groupGutter) * !stacked;
                    }

                    Ext.apply(barAttr, {
                        height: height,
                        width: mmax(bounds.groupBarWidth, 0),
                        x: xPos,
                        y: bottom - height
                    });
                }
                else {

                    offset = (total - 1) - i;
                    width = height + (bottom == bounds.zero);
                    xPos = bottom + (bottom != bounds.zero);

                    if (reverse) {

                        xPos = bounds.zero + bbox.width - width - (usedWidth === 0 ? 1 : 0);
                        if (stacked) {
                            xPos -= usedWidth;
                            usedWidth += width;
                        }
                    }

                    if (me.configuredColumnGirth && bounds.barsLoc.length) {
                        yPos = bounds.barsLoc[i] +
                            counter * bounds.groupBarWidth * (1 + groupGutter) * !stacked;
                    }
                    else {
                        yPos = bbox.y + padding.top +
                            (barWidth - shrunkBarWidth) * 0.5 +
                            offset * barWidth * (1 + gutter) +
                            counter * bounds.groupBarWidth *
                            (1 + groupGutter) * !stacked + 1;
                    }

                    Ext.apply(barAttr, {
                        height: mmax(bounds.groupBarWidth, 0),
                        width: width,
                        x: xPos,
                        y: yPos
                    });
                }
                if (height < 0) {
                    if (column) {
                        barAttr.y = top;
                        barAttr.height = mabs(height);
                    } else {
                        barAttr.x = top + height;
                        barAttr.width = mabs(height);
                    }
                }
                if (stacked) {
                    if (height < 0) {
                        top += height * (column ? -1 : 1);
                    } else {
                        bottom += height * (column ? -1 : 1);
                    }
                    totalDim += mabs(height);
                    if (height < 0) {
                        totalNegDim += mabs(height);
                    }
                }
                barAttr.x = Math.floor(barAttr.x) + 1;
                floorY = Math.floor(barAttr.y);
                if (Ext.isIE8m && barAttr.y > floorY) {
                    floorY--;
                }
                barAttr.y = floorY;
                barAttr.width = Math.floor(barAttr.width);
                barAttr.height = Math.floor(barAttr.height);
                items.push({
                    series: me,
                    yField: yFields[j],
                    storeItem: record,
                    value: [record.get(me.xField), yValue],
                    attr: barAttr,
                    point: column ? [barAttr.x + barAttr.width / 2, yValue >= 0 ? barAttr.y : barAttr.y + barAttr.height] :
                        [yValue >= 0 ? barAttr.x + barAttr.width : barAttr.x, barAttr.y + barAttr.height / 2]
                });

                if (animate && chart.resizing) {
                    attrs = column ? {
                        x: barAttr.x,
                        y: bounds.zero,
                        width: barAttr.width,
                        height: 0
                    } : {
                            x: bounds.zero,
                            y: barAttr.y,
                            width: 0,
                            height: barAttr.height
                        };
                    if (enableShadows && (stacked && !hasShadow || !stacked)) {
                        hasShadow = true;

                        for (shadowIndex = 0; shadowIndex < shadowGroupsLn; shadowIndex++) {
                            shadow = shadowGroups[shadowIndex].getAt(stacked ? i : (i * barsLen + j));
                            if (shadow) {
                                shadow.setAttributes(attrs, true);
                            }
                        }
                    }

                    sprite = group.getAt(i * barsLen + j);
                    if (sprite) {
                        sprite.setAttributes(attrs, true);
                    }
                }
                counter++;
            }
            if (stacked && items.length) {
                items[i * counter].totalDim = totalDim;
                items[i * counter].totalNegDim = totalNegDim;
                items[i * counter].totalPositiveValues = totalPositiveValues;
                items[i * counter].totalNegativeValues = totalNegativeValues;
            }
        }
        if (stacked && counter == 0) {

            for (i = 0, total = data.length; i < total; i++) {
                for (shadowIndex = 0; shadowIndex < shadowGroupsLn; shadowIndex++) {
                    shadow = shadowGroups[shadowIndex].getAt(i);
                    if (shadow) {
                        shadow.hide(true);
                    }
                }
            }
        }
    },
})
