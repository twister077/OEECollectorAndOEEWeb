#!/usr/bin/env python
# -*- coding: utf-8 -*-
import pygal
from gluon import *
from pygal.style import Style
from datetime import datetime
from datetime import timedelta

class Graphs(object):
    def __init__(self):
        self.dboee = DAL('sqlite://oee.db', pool_size=0, migrate=False)
        self.session = current.session
        self.request = current.request
        self.response = current.response
        self.cache = current.cache
        self.define_table()

    def define_table(self):
        self.tblOee_Reg = self.dboee.define_table('tblOee_Reg', \
                   Field('fldOeeRegTableKeyID', 'id', readable=False), \
                   Field('fldOeeRegNr', 'integer', label='Reg nr'), \
                   Field('fldOeeMachineCode', 'integer', label='Machine code'), \
                   Field('fldOeeMachineID', 'integer', label='Machine ID'), \
                   Field('fldOeeMachineDescription', 'string', label='Machine'), \
                   Field('fldOeeMachineStatusID', 'integer', label='Machine status ID'), \
                   Field('fldOeeMachineStatusDescription', 'string', label='Machine status description'), \
                   Field('fldOeeCountryID', 'integer', label='Country ID'), \
                   Field('fldOeeCountryDescription', 'string', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant ID'), \
                   Field('fldOeePlantDescription', 'string', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant ID'), \
                   Field('fldOeeSubPlantDescription', 'string', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department ID'), \
                   Field('fldOeeDepartmentDescription', 'string', label='Department'), \
                   Field('fldOeeStartDateTime', 'datetime', label='Start date'), \
                   Field('fldOeeEndDateTime', 'datetime', label='End date'), \
                   Field('fldOeeActivityDuration', 'integer', label='Duration'), \
                   Field('fldOeeTeamID', 'integer', label='Team ID'), \
                   Field('fldOeeTeamDescription', 'string', label='Team'), \
                   Field('fldOeeTeamColorID', 'integer', label='Team color ID'), \
                   Field('fldOeeTeamColorDescription', 'string', label='Team color'), \
                   Field('fldOeeShiftTimeID', 'integer', label='Shift ID'), \
                   Field('fldOeeShiftTimeDescription', 'string', label='Shift'), \
                   Field('fldOeeShiftStartDateTime', 'datetime', label='Shift starttime'), \
                   Field('fldOeeShiftEndDateTime', 'datetime', label='Shift endtime'), \
                   Field('fldOeeShiftDuration', 'integer', label='Shift duration'), \
                   Field('fldOeeAverageSpeed', 'integer', label='Average speed'), \
                   Field('fldOeeNormSpeed', 'integer', label='Norm speed'), \
                   Field('fldOeeCounter', 'integer', label='Counter'), \
                   Field('fldOeeCounterUnitID', 'integer', label='Counter-unit ID'), \
                   Field('fldOeeCounterUnitDescription', 'string', label='Counter-unit'), \
                   Field('fldOeeActivityGroupID', 'integer', label='Activitygroup ID'), \
                   Field('fldOeeActivityGroupDescription', 'string', label='Activitygroup'), \
                   Field('fldOeeActivityID', 'integer', label='Activity ID'), \
                   Field('fldOeeActivityDescription', 'string', label='Activity'), \
                   Field('fldOeeArticleNr', 'string', label='Article nr'), \
                   Field('fldOeeArticleDescription', 'string', label='Article description'), \
                   Field('fldOeeOrderNr', 'string', label='Order nr'), \
                   Field('fldOeeOrderDescription', 'string', label='Order description'), \
                   Field('fldOeeUserLogInformation', 'string', label='Activity log'), \
                   Field('fldOeeUserShiftLogInformation', 'string', label='Shift log'), \
                   Field('fldOeeCurrentPerformance', 'integer', label='Performance'), \
                   Field('fldOeeCurrentAvailability', 'integer', label='Availability'), \
                   Field('fldOeeCurrentQuality', 'integer', label='Quality'), \
                   Field('fldOeeCurrentOee', 'integer', label='OEE'), \
                   Field('fldOeeActivityGroupCalcForOee', 'integer', label='Calculate for OEE'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified'), \
                   Field('fldOeeSyncDate', 'datetime'), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

        self.tblOee_Progress = self.dboee.define_table('tblOee_Progress', \
                   Field('fldOeeProgressTableKeyID', 'id', readable=False), \
                   Field('fldOeeRegID', 'integer', label='Reg nr'), \
                   Field('fldOeeStartDateTime', 'datetime'), \
                   Field('fldOeeActivityDuration', 'integer'), \
                   Field('fldOeeCounter', 'integer'), \
                   Field('fldOeeNormSpeed', 'integer'), \
                   Field('fldOeeCountryID', 'integer'), \
                   Field('fldOeePlantID', 'integer'), \
                   Field('fldOeeSubPlantID',  'integer'), \
                   Field('fldOeeDepartmentID', 'integer'), \
                   Field('fldOeeCurrentOee', 'integer'), \
                   Field('fldOeeCurrentAvailability', 'integer'), \
                   Field('fldOeeCurrentPerformance', 'integer'), \
                   Field('fldOeeCurrentQuality', 'integer'), \
                   Field('fldOeeRegHistory', 'boolean'), \
                   Field('fldOeeDateModified', 'datetime'), \
                   Field('fldOeeMachineID', 'integer'), \
                   Field('fldOeeSyncDate', 'datetime'), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

    def oeegraph(self):
        intSize = 0
        intMachineID = int(self.request.vars['machineid'])
        intCountryNr = int(self.request.vars['countrynr'])
        intPlantNr = int(self.request.vars['plantnr'])
        intSubPlantNr = int(self.request.vars['subplantnr'])
        intDepartmentNr = int(self.request.vars['departmentnr'])
        datFrom = datetime.now()
        datTo = datetime.now()
        try:
            intRange = int(self.request.vars['range'])
        except:
            intRange = 0
            datFrom = 0
            datTo = 0
        try:
            intPerformance = int(self.request.vars['performance'])
            intAvailability = int(self.request.vars['availability'])
            intQuality = int(self.request.vars['quality'])
            intSize = int(self.request.vars['size'])
        except:
            intPerformance = 0
            intAvailability = 0
            intQuality = 0
        try:
            intFrom = int(self.request.vars['fromdate'])
            intTo = int(self.request.vars['todate'])
            datFrom = (datetime(1970,1,1) + timedelta(0, intFrom))
            datTo = (datetime(1970,1,1) + timedelta(0, intTo))
        except:
            pass
        if intSize == 0:
            intHeight = 170
            intWidth = 190
            intRoundedBars = 7
        elif intSize == 1:
            intHeight = 77
            intWidth = 110
            intRoundedBars = 3
        self.response.headers['Content-Type']='image/svg+xml'
        style = Style(
            background='white')
        bar_chart = pygal.Bar(style=style, \
                              height=intHeight, \
                              width=intWidth, \
                              show_legend=False, \
                              show_y_guides=False, \
                              show_y_labels=False, \
                              rounded_bars=intRoundedBars)
        for row in self.dboee((self.dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                         (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                         (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                         (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                         (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                         (self.dboee.tblOee_Reg.fldOeeStartDateTime > datFrom)).select(self.dboee.tblOee_Reg.ALL, \
                                                                                  orderby=~self.dboee.tblOee_Reg.fldOeeRegNr, \
                                                                                  limitby=(0,1)):
            intPerformance = row.fldOeeCurrentPerformance
            if intPerformance > 100:
                intPerformance = 100
            intAvailability = row.fldOeeCurrentAvailability
            intQuality = row.fldOeeCurrentQuality
            intOEE = (intPerformance + intAvailability + intQuality) / 3
            bar_chart.add('Availability', [{'value': intAvailability, 'color': 'yellow'}])
            bar_chart.add('Performance', [{'value': intPerformance, 'color': 'red'}])
            bar_chart.add('Quality', [{'value': intQuality, 'color': 'blue'}])
            bar_chart.add('OEE', [{'value': intOEE, 'color': 'green'}])
        if intPerformance + intAvailability + intQuality == 0:
            bar_chart.add('Performance', [{'value': 100, 'color': 'blue'}])
            bar_chart.add('Availability', [{'value': 100, 'color': 'yellow'}])
            bar_chart.add('Quality', [{'value': 100, 'color': 'red'}])
            bar_chart.add('OEE', [{'value': 100, 'color': 'green'}])
        return bar_chart.render()

    def activitygraph(self):
        intMachineID = int(self.request.vars['machineid'])
        intCountryNr = int(self.request.vars['countrynr'])
        intPlantNr = int(self.request.vars['plantnr'])
        intSubPlantNr = int(self.request.vars['subplantnr'])
        intDepartmentNr = int(self.request.vars['departmentnr'])
        intRange = int(self.request.vars['range'])
        intFrom = 0
        intTo = 0
        datFrom = datetime.now()
        datTo = datetime.now()
        try:
            intFrom = int(self.request.vars['fromdate'])
            intTo = int(self.request.vars['todate'])
            datFrom = (datetime(1970,1,1) + timedelta(0, intFrom))
            datTo = (datetime(1970,1,1) + timedelta(0, intTo))
        except:
            pass
        self.response.headers['Content-Type']='image/svg+xml'
        style = Style(
            background='transparent', \
            plot_background='transparent', \
            no_data_font_size=12)
        bar_chart = pygal.Bar(style=style, \
                              include_y_axis=True, \
                              legend_at_bottom=True, \
                              height=500, \
                              width=1100, \
                              show_y_guides=False, \
                              rounded_bars=7, \
                              truncate_legend=21)
        intRegEndNr = 0
        try:
            intRegEndNr = self.dboee((self.dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                                     (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                                     (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                                     (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                                     (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                                     (self.dboee.tblOee_Reg.fldOeeStartDateTime <= datTo) & \
                                     (self.dboee.tblOee_Reg.fldOeeEndDateTime >= datTo)).select(orderby=self.dboee.tblOee_Reg.fldOeeRegNr, \
                                                                                                              limitby=(0,1))[0].fldOeeRegNr
        except:
            #todo: gaat goed fout hier?!? datTo wordt niet goed gebruikt op de een of andere wijze.
            rows = self.dboee((self.dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                              (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                              (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                              (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                              (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                              (self.dboee.tblOee_Reg.fldOeeStartDateTime <= datTo)).select(self.dboee.tblOee_Reg.ALL, \
                                                                                                      orderby=~self.dboee.tblOee_Reg.fldOeeRegNr, \
                                                                                                      limitby=(0,1))
            for row in rows:
                intRegEndNr = row.fldOeeRegNr
        #return str(datTo) + '  -  ' + str(intRegEndNr)
        rows = self.dboee((self.dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                          (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                          (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                          (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                          (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                         ~(self.dboee.tblOee_Reg.fldOeeActivityDuration == 0) & \
                         ((self.dboee.tblOee_Reg.fldOeeMachineStatusID == 3) | \
                          (self.dboee.tblOee_Reg.fldOeeMachineStatusID == 4) | \
                          (self.dboee.tblOee_Reg.fldOeeMachineStatusID == 5)) & \
                          (self.dboee.tblOee_Reg.fldOeeStartDateTime >= datFrom) & \
                          (self.dboee.tblOee_Reg.fldOeeEndDateTime <= datTo)).select(self.dboee.tblOee_Reg.ALL, \
                                                                                    orderby=~self.dboee.tblOee_Reg.fldOeeActivityDuration, \
                                                                                    groupby=self.dboee.tblOee_Reg.fldOeeActivityDescription, \
                                                                                    limitby=(0,10))
        for row in rows:
            bar_chart.add(row.fldOeeActivityDescription, \
                          [{'label': row.fldOeeActivityDescription, \
                            'value': (row.fldOeeActivityDuration / 60)}])
        return bar_chart.render()

    def tfhoursbar(self):
        #datSearchStartDate = datetime(2015, 11, 6, 11, 30)
        #datSearchEndDate = datetime(2015, 11, 6, 21, 0)
        datSearchEndDate = datetime.now()
        datSearchStartDate = datSearchEndDate - timedelta(days=1)
        try:
            intFrom = int(self.request.vars['fromdate'])
            intTo = int(self.request.vars['todate'])
            datSearchStartDate = (datetime(1970,1,1) + timedelta(0, intFrom))
            datSearchEndDate = (datetime(1970,1,1) + timedelta(0, intTo))
        except:
            pass
        intRegStartNr = 0
        intRegEndNr = 0
        intMachineID = int(self.request.vars['machineid'])
        intCountryNr = int(self.request.vars['countrynr'])
        intPlantNr = int(self.request.vars['plantnr'])
        intSubPlantNr = int(self.request.vars['subplantnr'])
        intDepartmentNr = int(self.request.vars['departmentnr'])
        self.response.headers['Content-Type']='image/svg+xml'
        style = Style(background='transparent', \
                      no_data_font_size=12)
        tfhourbar_chart = pygal.HorizontalStackedBar(style = style, \
                                                height=90, \
                                                width=1000, \
                                                show_x_labels=True, \
                                                show_y_labels=False, \
                                                show_legend=False, \
                                                show_y_guides=False)

        try:
            intRegStartNr = self.dboee((self.dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                                       (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                                       (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                                       (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                                       (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                                      ((self.dboee.tblOee_Reg.fldOeeStartDateTime <= datSearchStartDate) & \
                                       (self.dboee.tblOee_Reg.fldOeeEndDateTime >= datSearchStartDate))).select(self.dboee.tblOee_Reg.ALL, \
                                                                                                                orderby=self.dboee.tblOee_Reg.fldOeeRegNr, \
                                                                                                                limitby=(0,1))[0].fldOeeRegNr
        except:
            rows = self.dboee((self.dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                             (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                             (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                             (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                             (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                             (self.dboee.tblOee_Reg.fldOeeStartDateTime <= datSearchStartDate)).select(self.dboee.tblOee_Reg.ALL, \
                                                                                                       orderby=~self.dboee.tblOee_Reg.fldOeeRegNr, \
                                                                                                       limitby=(0,1))
            for row in rows:
                intRegStartNr = row.fldOeeRegNr
        try:
            intRegEndNr = self.dboee((self.dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                                     (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                                     (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                                     (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                                     (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                                     (self.dboee.tblOee_Reg.fldOeeStartDateTime <= datSearchEndDate) & \
                                     (self.dboee.tblOee_Reg.fldOeeEndDateTime >= datSearchEndDate)).select(self.dboee.tblOee_Reg.ALL, \
                                                                                                              orderby=self.dboee.tblOee_Reg.fldOeeRegNr, \
                                                                                                              limitby=(0,1))[0].fldOeeRegNr
        except:
            rows = self.dboee((self.dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                              (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                              (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                              (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                              (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                              (self.dboee.tblOee_Reg.fldOeeStartDateTime <= datSearchEndDate)).select(self.dboee.tblOee_Reg.ALL, \
                                                                                                      orderby=~self.dboee.tblOee_Reg.fldOeeRegNr, \
                                                                                                      limitby=(0,1))
            for row in rows:
                intRegEndNr = row.fldOeeRegNr
        try:
            intRowCount = self.dboee((self.dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                              (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                              (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                              (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                              (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                              (self.dboee.tblOee_Reg.fldOeeRegNr >= intRegStartNr) & \
                              (self.dboee.tblOee_Reg.fldOeeRegNr <= intRegEndNr)).count()
        except:
            intRowCount = 0

        rows = self.dboee((self.dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                          (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                          (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                          (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                          (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                          (self.dboee.tblOee_Reg.fldOeeRegNr >= intRegStartNr) & \
                          (self.dboee.tblOee_Reg.fldOeeRegNr <= intRegEndNr)).select(self.dboee.tblOee_Reg.ALL, \
                                                                                                orderby=self.dboee.tblOee_Reg.fldOeeRegNr)
        datStart = datetime.now()
        datEnd = datetime.now()
        intSeconds = 0
        intX = 0
        strTest = ""
        for row in rows:
            if intX == 0:
                datEnd = row.fldOeeStartDateTime
                intX += 1
            datStart = row.fldOeeStartDateTime
            datDifference = datStart - datEnd
            intSeconds = datDifference.seconds
            if intSeconds >= 10:
                tfhourbar_chart.add('No registered data', [{'value': intSeconds /60, 'color': 'rgba(115, 77, 77, 1)'}])
            try:
                datEnd = row.fldOeeEndDateTime
                if datEnd is None:
                    datEnd = row.fldOeeStartDateTime + timedelta(minutes=int(row.fldOeeActivityDuration) / 60)
            except:
                datEnd = row.fldOeeStartDateTime
            strColor = self.mfColor(row.fldOeeMachineStatusID, row.fldOeeActivityGroupID)
            if intX == intRowCount - 1:
                datEnd = datSearchEndDate
                datDifference = datEnd - datStart
                intSeconds = datDifference.seconds
                bar_chart.add('SQL data', [{'value': intSeconds /60, 'color': strColor}])
            else:
                strColor = self.mfColor(row.fldOeeMachineStatusID, row.fldOeeActivityGroupID)
                tfhourbar_chart.add(row.fldOeeActivityDescription, [{'value': (row.fldOeeActivityDuration / 60), 'color': strColor}])
        return tfhourbar_chart.render()

    def oeeprogress(self):
        intMachineID = 0
        intCountryNr = 0
        intPlantNr = 0
        intSubPlantNr = 0
        intDepartmentNr = 0
        intHeight = 500
        intWidth = 1100
        intFrom = 0
        intTo = 0
        datFrom = datetime.now()
        datTo = datetime.now()
        intRange = 1
        self.response.headers['Content-Type']='image/svg+xml'
        try:
            intMachineID = int(self.request.vars['machineid'])
            intCountryNr = int(self.request.vars['countrynr'])
            intPlantNr = int(self.request.vars['plantnr'])
            intSubPlantNr = int(self.request.vars['subplantnr'])
            intDepartmentNr = int(self.request.vars['departmentnr'])
            intScreenWidth = int(self.request.vars['screenwidth'])
        except:
            pass
        try:
            intFrom = int(self.request.vars['fromdate'])
            intTo = int(self.request.vars['todate'])
            datFrom = (datetime(1970,1,1) + timedelta(0, intFrom))
            datTo = (datetime(1970,1,1) + timedelta(0, intTo))
            intRange = int(self.request.vars['range'])
        except:
            pass
        #((self.dboee.tblOee_Reg.fldOeeMachineStatusID == 1) | (self.dboee.tblOee_Reg.fldOeeMachineStatusID == 2))
        if intRange == 1:
            rows = self.dboee((self.dboee.tblOee_Progress.fldOeeMachineID == intMachineID) & \
                         (self.dboee.tblOee_Progress.fldOeeRegID == self.dboee.tblOee_Reg.fldOeeRegNr) & \
                         (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                         (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                         (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                         (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                         (self.dboee.tblOee_Progress.fldOeeStartDateTime >= datFrom)).select(self.dboee.tblOee_Progress.ALL)
        else:
            rows = self.dboee((self.dboee.tblOee_Progress.fldOeeMachineID == intMachineID) & \
                         (self.dboee.tblOee_Progress.fldOeeRegID == self.dboee.tblOee_Reg.fldOeeRegNr) & \
                         (self.dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                         (self.dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                         (self.dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                         (self.dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                         (self.dboee.tblOee_Progress.fldOeeStartDateTime >= datFrom) & \
                         (self.dboee.tblOee_Progress.fldOeeStartDateTime <= datTo)).select(self.dboee.tblOee_Progress.ALL)

        style = Style(
        background='white')
        line_chart = pygal.Line(style=style, \
                                  height=intHeight, \
                                  width=intWidth, \
                                  show_y_guides=False, \
                                  show_y_labels=False, \
                                  legend_at_bottom=True, \
                                  dots_size=0.1)

        lstRegIDs = []
        lstAvailability = []
        lstPerformance = []
        lstQuality = []
        lstOEE = []
        for row in rows:
            #if datestart new minus dateend old is greater than 10, add empty range?!?
            #how empty range?!?
            #first define scale then add valuepoint when value is in range
            lstRegIDs.append(row.fldOeeStartDateTime)
            lstAvailability.append(row.fldOeeCurrentAvailability)
            if row.fldOeeCurrentPerformance > 100:
                lstPerformance.append(100)
            else:
                lstPerformance.append(row.fldOeeCurrentPerformance)
            lstQuality.append(row.fldOeeCurrentQuality)
            if row.fldOeeCurrentOee > 100:
                lstOEE.append(100)
            else:
                lstOEE.append(row.fldOeeCurrentOee)
        line_chart.x_labels = lstRegIDs
        line_chart.add('Availability', lstAvailability)
        line_chart.add('Performance', lstPerformance)
        line_chart.add('Quality', lstQuality)
        line_chart.add('OEE', lstOEE)
        return line_chart.render()
    
    def mfColor(self, intStatusID, intGroupID):
        strColor = ''
        if intStatusID == 1:
            strColor = 'purple'
        elif intStatusID == 2:
            strColor = 'green'
        elif intStatusID == 3:
            strColor = 'pink'
        elif intStatusID == 4:
            strColor = 'brown'
        elif intStatusID == 5:
            if intGroupID == 1:
                strColor = 'orange'
            elif intGroupID == 2:
                strColor = 'red'
            elif intGroupID == 3:
                strColor = 'blue'
            elif intGroupID == 4:
                strColor = 'yellow'
        return strColor