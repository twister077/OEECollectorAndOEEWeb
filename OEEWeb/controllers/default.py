import os
import pygal
import shutil
from pygal.style import Style
from random import randint
from datetime import timedelta
from datetime import time
from datetime import datetime
from datetime import date
from graphs import Graphs
from dbtools import DbTools

gintMachineID = 0
gintCountryNr = 0
gintPlantNr = 0
gintSubPlantNr = 0
gintDepartmentNr = 0
gintDefMachID = 3

service = Service()

@auth.requires_login()
def index():
    gintCountryNr = 0
    gintPlantNr = 0
    gintSubPlantNr = 0
    gintDepartmentNr = 0
    intScreenWidth = 0
    arrViews = dict()
    strLevel = ''
    strLevelUrl = ''
    strCountry = ''
    strPlant = ''
    strSubPlant = ''
    strDepartment = ''
    formtable = ''
    try:
        intScreenWidth = int(request.vars['screenwidth'])
        gintCountryNr = int(request.vars['countrynr'])
        gintPlantNr = int(request.vars['plantnr'])
        gintSubPlantNr = int(request.vars['subplantnr'])
        gintDepartmentNr = int(request.vars['departmentnr'])
    except:
        pass
    blnError = False
    try:
        if gintCountryNr == 0:
            if auth.user.fldOeeUserRightID <= 10:
                arrViews = dboee(dboee.tblOee_Country).select(dboee.tblOee_Country.fldOeeCountryNr, \
                                                              dboee.tblOee_Country.fldOeeCountryDescription, \
                                                              dboee.tblOee_Country.fldOeeCountryInformation, \
                                                              orderby=dboee.tblOee_Country.fldOeeCountryDescription)
            else:
                arrViews = dboee((dboee.tblOee_Country.fldOeeCountryNr == auth.user.fldOeeCountryID)).select(dboee.tblOee_Country.fldOeeCountryNr, \
                                                              dboee.tblOee_Country.fldOeeCountryDescription, \
                                                              dboee.tblOee_Country.fldOeeCountryInformation, \
                                                              orderby=dboee.tblOee_Country.fldOeeCountryDescription)
            strLevelUrl = 'index?screenwidth=' + str(intScreenWidth) + '&countrynr='
            strLevel = 'Country'
        else:
            if gintPlantNr == 0:
                if auth.user.fldOeeUserRightID <= 20:
                    arrViews = dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr).select(dboee.tblOee_Plant.fldOeePlantNr, \
                                                                                                 dboee.tblOee_Plant.fldOeePlantDescription, \
                                                                                                 dboee.tblOee_Plant.fldOeePlantInformation, \
                                                                                                 orderby=dboee.tblOee_Plant.fldOeePlantDescription)
                else:
                    arrViews = dboee((dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr) & \
                                     (dboee.tblOee_Plant.fldOeePlantNr == auth.user.fldOeePlantID)).select(dboee.tblOee_Plant.fldOeePlantNr, \
                                                                                                           dboee.tblOee_Plant.fldOeePlantDescription, \
                                                                                                           dboee.tblOee_Plant.fldOeePlantInformation, \
                                                                                                           orderby=dboee.tblOee_Plant.fldOeePlantDescription)
                strCountry = dboee(dboee.tblOee_Country.fldOeeCountryNr == gintCountryNr).select()[0].get('fldOeeCountryDescription')
                strLevelUrl = 'index?screenwidth=' + str(intScreenWidth) + '&countrynr=' + str(gintCountryNr) + '&plantnr='
                strLevel = 'Plant'
            else:
                if gintSubPlantNr == 0:
                    if auth.user.fldOeeUserRightID <= 30:
                        arrViews = dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                         (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)).select(dboee.tblOee_SubPlant.fldOeeSubPlantNr, \
                                                                                                      dboee.tblOee_SubPlant.fldOeeSubPlantDescription, \
                                                                                                      dboee.tblOee_SubPlant.fldOeeSubPlantInformation, \
                                                                                                      orderby=dboee.tblOee_SubPlant.fldOeeSubPlantDescription)
                    else:
                        arrViews = dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                         (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr) & \
                                         (dboee.tblOee_SubPlant.fldOeeSubPlantNr == auth.user.fldOeeSubPlantID)).select(dboee.tblOee_SubPlant.fldOeeSubPlantNr, \
                                                                                                                        dboee.tblOee_SubPlant.fldOeeSubPlantDescription, \
                                                                                                                        dboee.tblOee_SubPlant.fldOeeSubPlantInformation, \
                                                                                                                        orderby=dboee.tblOee_SubPlant.fldOeeSubPlantDescription)
                    strCountry = dboee(dboee.tblOee_Country.fldOeeCountryNr == gintCountryNr).select()[0].get('fldOeeCountryDescription')
                    strPlant = dboee(dboee.tblOee_Plant.fldOeePlantNr == gintPlantNr).select()[0].get('fldOeePlantDescription')
                    strLevelUrl = 'index?screenwidth=' + str(intScreenWidth) + '&countrynr=' + str(gintCountryNr) + '&plantnr=' + str(gintPlantNr) + '&subplantnr='
                    strLevel = 'SubPlant'
                else:
                    if gintDepartmentNr == 0:
                        if auth.user.fldOeeUserRightID <= 40:
                            arrViews = dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                             (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                             (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)).select(dboee.tblOee_Department.fldOeeDepartmentNr, \
                                                                                                                  dboee.tblOee_Department.fldOeeDepartmentDescription, \
                                                                                                                  dboee.tblOee_Department.fldOeeDepartmentInformation, \
                                                                                                                  orderby=dboee.tblOee_Department.fldOeeDepartmentDescription)
                        else:
                            arrViews = dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                             (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                             (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr) & \
                                             (dboee.tblOee_Department.fldOeeDepartmentNr == auth.user.fldOeeDepartmentID)).select(dboee.tblOee_Department.fldOeeDepartmentNr, \
                                                                                                                  dboee.tblOee_Department.fldOeeDepartmentDescription, \
                                                                                                                  dboee.tblOee_Department.fldOeeDepartmentInformation, \
                                                                                                                  orderby=dboee.tblOee_Department.fldOeeDepartmentDescription)
                        strCountry = dboee(dboee.tblOee_Country.fldOeeCountryNr == gintCountryNr).select()[0].get('fldOeeCountryDescription')
                        strPlant = dboee(dboee.tblOee_Plant.fldOeePlantNr == gintPlantNr).select()[0].get('fldOeePlantDescription')
                        strSubPlant = dboee(dboee.tblOee_SubPlant.fldOeeSubPlantNr == gintSubPlantNr).select()[0].get('fldOeeSubPlantDescription')
                        strLevelUrl = 'machselect?screenwidth=' + str(intScreenWidth) + '&countrynr=' + str(gintCountryNr) + '&plantnr=' + str(gintPlantNr) + '&subplantnr=' + str(gintSubPlantNr) + '&departmentnr='
                        strLevel = 'Department'
                    else:
                        if auth.user.fldOeeUserRightID <= 50:
                            arrViews = dboee((dboee.tblOee_Machine.fldOeeCountryID == gintCountryNr) & \
                                             (dboee.tblOee_Machine.fldOeePlantID == gintPlantNr) & \
                                             (dboee.tblOee_Machine.fldOeeSubPlantID == gintSubPlantNr) & \
                                             (dboee.tblOee_Machine.fldOeeDepartmentID == gintDepartmentNr)).select(dboee.tblOee_Machine.fldOeeMachineNr, \
                                                                                                                   dboee.tblOee_Machine.fldOeeMachineDescription, \
                                                                                                                   dboee.tblOee_Machine.fldOeeMachineInformation)
                        else:
                            arrViews = dboee((dboee.tblOee_Machine.fldOeeCountryID == gintCountryNr) & \
                                             (dboee.tblOee_Machine.fldOeePlantID == gintPlantNr) & \
                                             (dboee.tblOee_Machine.fldOeeSubPlantID == gintSubPlantNr) & \
                                             (dboee.tblOee_Machine.fldOeeDepartmentID == gintDepartmentNr)).select(dboee.tblOee_Machine.fldOeeMachineNr, \
                                                                                                                   dboee.tblOee_Machine.fldOeeMachineDescription, \
                                                                                                                   dboee.tblOee_Machine.fldOeeMachineInformation)
                        #define including and and and
                        strCountry = dboee(dboee.tblOee_Country.fldOeeCountryNr == gintCountryNr).select()[0].get('fldOeeCountryDescription')
                        strPlant = dboee((dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr) & \
                                         (dboee.tblOee_Plant.fldOeePlantNr == gintPlantNr)).select()[0].get('fldOeePlantDescription')
                        strSubPlant = dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr) & \
                                            (dboee.tblOee_SubPlant.fldOeeSubPlantNr == gintSubPlantNr)).select()[0].get('fldOeeSubPlantDescription')
                        strDepartment = dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                              (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                              (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr) & \
                                              (dboee.tblOee_Department.fldOeeDepartmentNr == gintDepartmentNr)).select()[0].get('fldOeeDepartmentDescription')
                        strLevelUrl = 'machdetails?screenwidth=' + str(intScreenWidth) + '&country='
                        strLevel = 'Machine'
    except:
        blnError = True
    if blnError == True:
        redirect(URL('index'))
    return dict(arrViews = arrViews, \
                 strLevelUrl = strLevelUrl, \
                 gintCountryNr = gintCountryNr, \
                 gintPlantNr = gintPlantNr, \
                 gintSubPlantNr = gintSubPlantNr, \
                 gintDepartmentNr = gintDepartmentNr, \
                 strLevel = strLevel, \
                 strCountry = strCountry, \
                 strPlant = strPlant, \
                 strSubPlant = strSubPlant, \
                 strDepartment = strDepartment, \
                 intScreenWidth = intScreenWidth, \
                 formtable = formtable)

@auth.requires_login()
def add():
    gintCountryNr = 0
    gintPlantNr = 0
    gintSubPlantNr = 0
    gintDepartmentNr = 0
    form = SQLFORM.factory(Field('dummy', 'id'))
    row = ()
    strLevel = ''
    strCountry = ''
    strPlant = ''
    strSubPlant = ''
    try:
        intLevel = int(request.vars['lvl'])
    except:
        intLevel = 0
    try:
        strLevel = str(request.vars['level'])
        gintCountryNr = int(request.vars['countrynr'])
        gintPlantNr = int(request.vars['plantnr'])
        gintSubPlantNr = int(request.vars['subplantnr'])
        gintDepartmentNr = int(request.vars['departmentnr'])
    except:
        pass
    if strLevel == 'None':
        strLevel = 'Country'
    if strLevel == 'Country':
        row = dboee(dboee.tblOee_Country).select(orderby=~dboee.tblOee_Country.fldOeeCountryNr, limitby=(0,1))
        dboee.tblOee_Country.fldOeeCountryNr.default = row[0].fldOeeCountryNr + 1
        form = SQLFORM(dboee.tblOee_Country)
    if strLevel == 'Plant':
        row = dboee(dboee.tblOee_Plant).select(orderby=~dboee.tblOee_Plant.fldOeePlantNr, limitby=(0,1))
        dboee.tblOee_Plant.fldOeePlantNr.default = row[0].fldOeePlantNr + 1
        dboee.tblOee_Plant.fldOeeCountryID.default = gintCountryNr
        form = SQLFORM(dboee.tblOee_Plant)
    if strLevel == 'SubPlant':
        row = dboee(dboee.tblOee_SubPlant).select(orderby=~dboee.tblOee_SubPlant.fldOeeSubPlantNr, limitby=(0,1))
        dboee.tblOee_SubPlant.fldOeeSubPlantNr.default = row[0].fldOeeSubPlantNr + 1
        dboee.tblOee_SubPlant.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_SubPlant.fldOeePlantID.default = gintPlantNr
        if intLevel == 1:
            dboee.tblOee_SubPlant.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                          dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        form = SQLFORM(dboee.tblOee_SubPlant)
    if strLevel == 'Department':
        row = dboee(dboee.tblOee_Department).select(orderby=~dboee.tblOee_Department.fldOeeDepartmentNr, limitby=(0,1))
        dboee.tblOee_Department.fldOeeDepartmentNr.default = row[0].fldOeeDepartmentNr + 1
        dboee.tblOee_Department.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_Department.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_Department.fldOeeSubPlantID.default = gintSubPlantNr
        if intLevel == 1:
            dboee.tblOee_Department.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                      dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
            dboee.tblOee_Department.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr)), \
                                                                         dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        elif intLevel == 2:
            dboee.tblOee_Department.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                          dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
            dboee.tblOee_Department.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                               (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                         dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')

        form = SQLFORM(dboee.tblOee_Department)
    if strLevel == 'ActivityGroup':
        row = dboee(dboee.tblOee_ActivityGroup).select(orderby=~dboee.tblOee_ActivityGroup.fldOeeActivityGroupNr, limitby=(0,1))
        dboee.tblOee_ActivityGroup.fldOeeActivityGroupNr.default = row[0].fldOeeActivityGroupNr + 1
        dboee.tblOee_ActivityGroup.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_ActivityGroup.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_ActivityGroup.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_ActivityGroup.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_ActivityGroup.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                    dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_ActivityGroup.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                              (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                        dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_ActivityGroup.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                                (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                                (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        form = SQLFORM(dboee.tblOee_ActivityGroup)
    if strLevel == 'Activity':
        row = dboee(dboee.tblOee_Activity).select(orderby=~dboee.tblOee_Activity.fldOeeActivityNr, limitby=(0,1))
        dboee.tblOee_Activity.fldOeeActivityNr.default = row[0].fldOeeActivityNr + 1
        dboee.tblOee_Activity.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_Activity.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_Activity.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_Activity.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_Activity.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_Activity.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_Activity.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        dboee.tblOee_Activity.fldOeeActivityGroupID.requires = IS_IN_DB(dboee((dboee.tblOee_ActivityGroup.fldOeeCountryID == gintCountryNr) & \
                                                                                (dboee.tblOee_ActivityGroup.fldOeePlantID == gintPlantNr) & \
                                                                                (dboee.tblOee_ActivityGroup.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                                (dboee.tblOee_ActivityGroup.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_ActivityGroup.fldOeeActivityGroupNr, '%(fldOeeActivityGroupDescription)s')
        form = SQLFORM(dboee.tblOee_Activity)
    if strLevel == 'MachineActivity':
        intMachineID = 0
        try:
            intMachineID = int(request.vars['machineid'])
        except:
            pass
        rows = dboee((dboee.tblOee_MachineActivity.fldOeeCountryID == gintCountryNr) & \
                     (dboee.tblOee_MachineActivity.fldOeePlantID == gintPlantNr) & \
                     (dboee.tblOee_MachineActivity.fldOeeSubPlantID == gintSubPlantNr) & \
                     (dboee.tblOee_MachineActivity.fldOeeDepartmentID == gintDepartmentNr) & \
                     (dboee.tblOee_MachineActivity.fldOeeMachineID == intMachineID)).select(orderby=~dboee.tblOee_MachineActivity.fldOeeMachineActivitySortOrder, limitby=(0,1))
        for row in rows:
            dboee.tblOee_MachineActivity.fldOeeMachineActivitySortOrder.default = rows[0].fldOeeMachineActivitySortOrder + 1
        lstSortOrder = []
        for intX in range(1, 101):
            lstSortOrder.append(intX)
        dboee.tblOee_MachineActivity.fldOeeMachineActivitySortOrder.requires = IS_IN_SET(lstSortOrder)
        dboee.tblOee_MachineActivity.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_MachineActivity.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_MachineActivity.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_MachineActivity.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_MachineActivity.fldOeeMachineID.default = intMachineID
        dboee.tblOee_MachineActivity.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_MachineActivity.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_MachineActivity.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        dboee.tblOee_MachineActivity.fldOeeMachineActivityID.requires = IS_IN_DB(dboee((dboee.tblOee_Activity.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Activity.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Activity.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Activity.fldOeeActivityNr, '%(fldOeeActivityDescription)s')
        dboee.tblOee_MachineActivity.fldOeeMachineID.requires = IS_IN_DB(dboee((dboee.tblOee_Machine.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Machine.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Machine.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_Machine.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_Machine.fldOeeMachineNr, '%(fldOeeMachineDescription)s')
        form = SQLFORM(dboee.tblOee_MachineActivity)
    if strLevel == 'Machines':
        row = dboee(dboee.tblOee_Machine).select(orderby=~dboee.tblOee_Machine.fldOeeMachineNr, limitby=(0,1))
        dboee.tblOee_Machine.fldOeeMachineNr.default = row[0].fldOeeMachineNr + 1
        dboee.tblOee_Machine.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_Machine.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_Machine.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_Machine.fldOeeDepartmentID.default = gintDepartmentNr
        rows = dboee((dboee.tblOee_Machine.fldOeeCountryID == gintCountryNr) & \
                     (dboee.tblOee_Machine.fldOeePlantID == gintPlantNr) & \
                     (dboee.tblOee_Machine.fldOeeSubPlantID == gintSubPlantNr) & \
                     (dboee.tblOee_Machine.fldOeeDepartmentID == gintDepartmentNr)).select(orderby=~dboee.tblOee_Machine.fldOeeMachineSortOrder, limitby=(0,1))
        for row in rows:
            dboee.tblOee_Machine.fldOeeMachineSortOrder.default = rows[0].fldOeeMachineSortOrder + 1
        lstSortOrder = []
        for intX in range(1, 101):
            lstSortOrder.append(intX)
        dboee.tblOee_Machine.fldOeeMachineSortOrder.requires = IS_IN_SET(lstSortOrder)
        dboee.tblOee_Machine.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_Machine.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_Machine.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        dboee.tblOee_Machine.fldOeeModuleID.requires = IS_IN_DB(dboee((dboee.tblOee_Module.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Module.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Module.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_Module.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_Module.fldOeeModuleNr, '%(fldOeeModuleDescription)s')
        dboee.tblOee_Machine.fldOeeMachineShortBreakID.requires = IS_IN_DB(dboee((dboee.tblOee_MachineShortbreak.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_MachineShortbreak.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_MachineShortbreak.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_MachineShortbreak.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_MachineShortbreak.fldOeeMachineShortBreakNr, '%(fldOeeMachineShortBreakDescription)s')
        dboee.tblOee_Machine.fldOeeMachineUndefinedProdID.requires = IS_IN_DB(dboee((dboee.tblOee_MachineUndefinedProduction.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_MachineUndefinedProduction.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_MachineUndefinedProduction.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_MachineUndefinedProduction.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_MachineUndefinedProduction.fldOeeMachineUndefinedProductionNr, '%(fldOeeMachineUndefinedProductionDescription)s')
        dboee.tblOee_Machine.fldOeeMachineUndefinedStandStillID.requires = IS_IN_DB(dboee((dboee.tblOee_MachineUndefinedStandstill.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_MachineUndefinedStandstill.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_MachineUndefinedStandstill.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_MachineUndefinedStandstill.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_MachineUndefinedStandstill.fldOeeMachineUndefinedStandstillNr, '%(fldOeeMachineUndefinedStandstillDescription)s')
        dboee.tblOee_Machine.fldOeeMachineUnscheduledID.requires = IS_IN_DB(dboee((dboee.tblOee_MachineUnscheduled.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_MachineUnscheduled.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_MachineUnscheduled.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_MachineUnscheduled.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_MachineUnscheduled.fldOeeMachineUnscheduledNr, '%(fldOeeMachineUnscheduledDescription)s')
        dboee.tblOee_Machine.fldOeeMachineIOFailureID.requires = IS_IN_DB(dboee((dboee.tblOee_MachineIOFailure.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_MachineIOFailure.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_MachineIOFailure.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_MachineIOFailure.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_MachineIOFailure.fldOeeMachineIOFailureNr, '%(fldOeeMachineIOFailureDescription)s')
        dboee.tblOee_Machine.fldOeeMachineUnitID.requires = IS_IN_DB(dboee((dboee.tblOee_MachineUnit.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_MachineUnit.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_MachineUnit.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_MachineUnit.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_MachineUnit.fldOeeMachineUnitNr, '%(fldOeeMachineUnitDescription)s')
        dboee.tblOee_Machine.fldOeeMachineTarget1OEE.requires = IS_IN_SET(lstSortOrder)
        dboee.tblOee_Machine.fldOeeMachineTarget2OEE.requires = IS_IN_SET(lstSortOrder)
        form = SQLFORM(dboee.tblOee_Machine)
    if strLevel == 'Shortbreaks':
        row = dboee(dboee.tblOee_MachineShortbreak).select(orderby=~dboee.tblOee_MachineShortbreak.fldOeeMachineShortBreakNr, limitby=(0,1))
        dboee.tblOee_MachineShortbreak.fldOeeMachineShortBreakNr.default = row[0].fldOeeMachineShortBreakNr + 1
        dboee.tblOee_MachineShortbreak.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_MachineShortbreak.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_MachineShortbreak.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_MachineShortbreak.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_MachineShortbreak.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_MachineShortbreak.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_MachineShortbreak.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        form = SQLFORM(dboee.tblOee_MachineShortbreak)
    if strLevel == 'UndefinedProduction':
        row = dboee(dboee.tblOee_MachineUndefinedProduction).select(orderby=~dboee.tblOee_MachineUndefinedProduction.fldOeeMachineUndefinedProductionNr, limitby=(0,1))
        dboee.tblOee_MachineUndefinedProduction.fldOeeMachineUndefinedProductionNr.default = row[0].fldOeeMachineUndefinedProductionNr + 1
        dboee.tblOee_MachineUndefinedProduction.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_MachineUndefinedProduction.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_MachineUndefinedProduction.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_MachineUndefinedProduction.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_MachineUndefinedProduction.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_MachineUndefinedProduction.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_MachineUndefinedProduction.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        form = SQLFORM(dboee.tblOee_MachineUndefinedProduction)
    if strLevel == 'UndefinedStandstill':
        row = dboee(dboee.tblOee_MachineUndefinedStandstill).select(orderby=~dboee.tblOee_MachineUndefinedStandstill.fldOeeMachineUndefinedStandstillNr, limitby=(0,1))
        dboee.tblOee_MachineUndefinedStandstill.fldOeeMachineUndefinedStandstillNr.default = row[0].fldOeeMachineUndefinedStandstillNr + 1
        dboee.tblOee_MachineUndefinedStandstill.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_MachineUndefinedStandstill.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_MachineUndefinedStandstill.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_MachineUndefinedStandstill.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_MachineUndefinedStandstill.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_MachineUndefinedStandstill.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_MachineUndefinedStandstill.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        form = SQLFORM(dboee.tblOee_MachineUndefinedStandstill)
    if strLevel == 'IOFailures':
        row = dboee(dboee.tblOee_MachineIOFailure).select(orderby=~dboee.tblOee_MachineIOFailure.fldOeeMachineIOFailureNr, limitby=(0,1))
        dboee.tblOee_MachineIOFailure.fldOeeMachineIOFailureNr.default = row[0].fldOeeMachineIOFailureNr + 1
        dboee.tblOee_MachineIOFailure.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_MachineIOFailure.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_MachineIOFailure.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_MachineIOFailure.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_MachineIOFailure.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_MachineIOFailure.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_MachineIOFailure.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        form = SQLFORM(dboee.tblOee_MachineIOFailure)
    if strLevel == 'Unscheduled':
        row = dboee(dboee.tblOee_MachineUnscheduled).select(orderby=~dboee.tblOee_MachineUnscheduled.fldOeeMachineUnscheduledNr, limitby=(0,1))
        dboee.tblOee_MachineUnscheduled.fldOeeMachineUnscheduledNr.default = row[0].fldOeeMachineUnscheduledNr + 1
        dboee.tblOee_MachineUnscheduled.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_MachineUnscheduled.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_MachineUnscheduled.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_MachineUnscheduled.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_MachineUnscheduled.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_MachineUnscheduled.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_MachineUnscheduled.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        form = SQLFORM(dboee.tblOee_MachineUnscheduled)
    if strLevel == 'Modules':
        row = dboee(dboee.tblOee_Module).select(orderby=~dboee.tblOee_Module.fldOeeModuleNr, limitby=(0,1))
        dboee.tblOee_Module.fldOeeModuleNr.default = row[0].fldOeeModuleNr + 1
        dboee.tblOee_Module.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_Module.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_Module.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_Module.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_Module.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_Module.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_Module.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        dboee.tblOee_Module.fldOeeModuleTypeID.requires = IS_IN_DB(dboee((dboee.tblOee_ModuleType.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_ModuleType.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_ModuleType.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_ModuleType.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_ModuleType.fldOeeModuleTypeNr, '%(fldOeeModuleTypeDescription)s')
        dboee.tblOee_Module.fldOeeModuleSensorStyleID.requires = IS_IN_DB(dboee((dboee.tblOee_ModuleSensorStyle.fldOeeCountryID == gintCountryNr)), \
                                                                        dboee.tblOee_ModuleSensorStyle.fldOeeModuleSensorStyleNr, '%(fldOeeModuleSensorStyleDescription)s')
        lstSortOrder = []
        for intX in range(1, 21):
            lstSortOrder.append(intX)
        dboee.tblOee_Module.fldOeeModuleComPort.requires = IS_IN_SET(lstSortOrder)
        form = SQLFORM(dboee.tblOee_Module)
    if strLevel == 'Shifts':
        intCount = 0
        intCount = dboee((dboee.tblOee_ShiftTime.fldOeeCountryID == gintCountryNr) & \
                    (dboee.tblOee_ShiftTime.fldOeePlantID == gintPlantNr) & \
                    (dboee.tblOee_ShiftTime.fldOeeSubPlantID == gintSubPlantNr) & \
                    (dboee.tblOee_ShiftTime.fldOeeDepartmentID == gintDepartmentNr)).count()
        dboee.tblOee_ShiftTime.fldOeeShiftTimeNr.default = intCount + 1
        dboee.tblOee_ShiftTime.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_ShiftTime.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_ShiftTime.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_ShiftTime.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_ShiftTime.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_ShiftTime.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_ShiftTime.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        form = SQLFORM(dboee.tblOee_ShiftTime)
    if strLevel == 'Teams':
        row = dboee(dboee.tblOee_Team).select(orderby=~dboee.tblOee_Team.fldOeeTeamNr, limitby=(0,1))
        dboee.tblOee_Team.fldOeeTeamNr.default = row[0].fldOeeTeamNr + 1
        dboee.tblOee_Team.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_Team.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_Team.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_Team.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_Team.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_Team.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_Team.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        form = SQLFORM(dboee.tblOee_Team)
    if strLevel == 'Scheduler':
        row = dboee(dboee.tblOee_DailySchedule).select(orderby=~dboee.tblOee_DailySchedule.fldOeeDailyScheduleNr, limitby=(0,1))
        dboee.tblOee_DailySchedule.fldOeeDailyScheduleNr.default = row[0].fldOeeDailyScheduleNr + 1
        dboee.tblOee_DailySchedule.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_DailySchedule.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_DailySchedule.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_DailySchedule.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_DailySchedule.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_DailySchedule.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_DailySchedule.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        dboee.tblOee_DailySchedule.fldOeeTeamID.requires = IS_IN_DB(dboee((dboee.tblOee_Team.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Team.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Team.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_Team.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_Team.fldOeeTeamNr, '%(fldOeeTeamDescription)s')
        dboee.tblOee_DailySchedule.fldOeeShiftTimeID.requires = IS_IN_DB(dboee((dboee.tblOee_ShiftTime.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_ShiftTime.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_ShiftTime.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_ShiftTime.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_ShiftTime.fldOeeShiftTimeNr, '%(fldOeeShiftTimeDescription)s')
        form = SQLFORM(dboee.tblOee_DailySchedule)
    if strLevel == 'Articles':
        dboee.tblOee_Article.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_Article.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_Article.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_Article.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_Article.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_Article.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_Article.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        form = SQLFORM(dboee.tblOee_Article)
    if strLevel == 'Orders':
        dboee.tblOee_Order.fldOeeCountryID.default = gintCountryNr
        dboee.tblOee_Order.fldOeePlantID.default = gintPlantNr
        dboee.tblOee_Order.fldOeeSubPlantID.default = gintSubPlantNr
        dboee.tblOee_Order.fldOeeDepartmentID.default = gintDepartmentNr
        dboee.tblOee_Order.fldOeePlantID.requires = IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                                                dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
        dboee.tblOee_Order.fldOeeSubPlantID.requires = IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
        dboee.tblOee_Order.fldOeeDepartmentID.requires = IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                                                        dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
        dboee.tblOee_Order.fldOeeArticleID.requires = IS_IN_DB(dboee((dboee.tblOee_Article.fldOeeCountryID == gintCountryNr) & \
                                                                            (dboee.tblOee_Article.fldOeePlantID == gintPlantNr) & \
                                                                            (dboee.tblOee_Article.fldOeeSubPlantID == gintSubPlantNr) & \
                                                                            (dboee.tblOee_Article.fldOeeDepartmentID == gintDepartmentNr)), \
                                                                        dboee.tblOee_Article.fldOeeArticleNr, '%(fldOeeArticleNr)s' + ' - ' + '%(fldOeeArticleDescription)s')
        form = SQLFORM(dboee.tblOee_Order)
    if form.process(onvalidation=add_form_processing).accepted:
        response.flash = strLevel + ' accepted. Please Refesh page.'
    return dict(form = form, \
                 row = row, \
                 strLevel = strLevel, \
                 gintCountryNr = gintCountryNr, \
                 gintPlantNr = gintPlantNr, \
                 gintSubPlantNr = gintSubPlantNr, \
                 gintDepartmentNr = gintDepartmentNr, \
                 strCountry = strCountry, \
                 strPlant = strPlant, \
                 strSubPlant = strSubPlant, \
                 intScreenWidth = 0)

def add_form_processing(form):
    if 'fldOeeMachineActivityID' in form.vars:
        intRowCount = 0
        intRowCount = dboee((dboee.tblOee_MachineActivity.fldOeeCountryID == form.vars.fldOeeCountryID) & \
                            (dboee.tblOee_MachineActivity.fldOeePlantID == form.vars.fldOeePlantID) & \
                            (dboee.tblOee_MachineActivity.fldOeeSubPlantID == form.vars.fldOeeSubPlantID) & \
                            (dboee.tblOee_MachineActivity.fldOeeDepartmentID == form.vars.fldOeeDepartmentID) & \
                            (dboee.tblOee_MachineActivity.fldOeeMachineID == form.vars.fldOeeMachineID) & \
                            (dboee.tblOee_MachineActivity.fldOeeMachineActivityID == form.vars.fldOeeMachineActivityID)).count()
        if intRowCount > 0:
            form.errors.fldOeeMachineActivityID = 'Duplicate entry detected'
    elif 'fldOeeMachineWorkstationDescription' in form.vars:
        intRowCount = 0
        intRowCount = dboee((dboee.tblOee_Machine.fldOeeCountryID == form.vars.fldOeeCountryID) & \
                            (dboee.tblOee_Machine.fldOeePlantID == form.vars.fldOeePlantID) & \
                            (dboee.tblOee_Machine.fldOeeSubPlantID == form.vars.fldOeeSubPlantID) & \
                            (dboee.tblOee_Machine.fldOeeDepartmentID == form.vars.fldOeeDepartmentID) & \
                            (dboee.tblOee_Machine.fldOeeModuleID == form.vars.fldOeeModuleID)).count()
        if intRowCount > 0:
            form.errors.fldOeeModuleID = 'Duplicate module usage detected'
    elif 'fldOeeMachineShortBreakDescription' in form.vars:
        intRowCount = 0
        intRowCount = dboee((dboee.tblOee_MachineShortbreak.fldOeeCountryID == form.vars.fldOeeCountryID) & \
                            (dboee.tblOee_MachineShortbreak.fldOeePlantID == form.vars.fldOeePlantID) & \
                            (dboee.tblOee_MachineShortbreak.fldOeeSubPlantID == form.vars.fldOeeSubPlantID) & \
                            (dboee.tblOee_MachineShortbreak.fldOeeDepartmentID == form.vars.fldOeeDepartmentID)).count()
        if intRowCount > 0:
            form.errors.fldOeeDepartmentID = 'Duplicate record detected'
    elif 'fldOeeMachineUndefinedProductionDescription' in form.vars:
        intRowCount = 0
        intRowCount = dboee((dboee.tblOee_MachineUndefinedProduction.fldOeeCountryID == form.vars.fldOeeCountryID) & \
                            (dboee.tblOee_MachineUndefinedProduction.fldOeePlantID == form.vars.fldOeePlantID) & \
                            (dboee.tblOee_MachineUndefinedProduction.fldOeeSubPlantID == form.vars.fldOeeSubPlantID) & \
                            (dboee.tblOee_MachineUndefinedProduction.fldOeeDepartmentID == form.vars.fldOeeDepartmentID)).count()
        if intRowCount > 0:
            form.errors.fldOeeDepartmentID = 'Duplicate record detected'
    elif 'fldOeeMachineUndefinedStandstillDescription' in form.vars:
        intRowCount = 0
        intRowCount = dboee((dboee.tblOee_MachineUndefinedStandstill.fldOeeCountryID == form.vars.fldOeeCountryID) & \
                            (dboee.tblOee_MachineUndefinedStandstill.fldOeePlantID == form.vars.fldOeePlantID) & \
                            (dboee.tblOee_MachineUndefinedStandstill.fldOeeSubPlantID == form.vars.fldOeeSubPlantID) & \
                            (dboee.tblOee_MachineUndefinedStandstill.fldOeeDepartmentID == form.vars.fldOeeDepartmentID)).count()
        if intRowCount > 0:
            form.errors.fldOeeDepartmentID = 'Duplicate record detected'
    elif 'fldOeeMachineIOFailureDescription' in form.vars:
        intRowCount = 0
        intRowCount = dboee((dboee.tblOee_MachineIOFailure.fldOeeCountryID == form.vars.fldOeeCountryID) & \
                            (dboee.tblOee_MachineIOFailure.fldOeePlantID == form.vars.fldOeePlantID) & \
                            (dboee.tblOee_MachineIOFailure.fldOeeSubPlantID == form.vars.fldOeeSubPlantID) & \
                            (dboee.tblOee_MachineIOFailure.fldOeeDepartmentID == form.vars.fldOeeDepartmentID)).count()
        if intRowCount > 0:
            form.errors.fldOeeDepartmentID = 'Duplicate record detected'
    elif 'fldOeeMachineUnscheduledDescription' in form.vars:
        intRowCount = 0
        intRowCount = dboee((dboee.tblOee_MachineUnscheduled.fldOeeCountryID == form.vars.fldOeeCountryID) & \
                            (dboee.tblOee_MachineUnscheduled.fldOeePlantID == form.vars.fldOeePlantID) & \
                            (dboee.tblOee_MachineUnscheduled.fldOeeSubPlantID == form.vars.fldOeeSubPlantID) & \
                            (dboee.tblOee_MachineUnscheduled.fldOeeDepartmentID == form.vars.fldOeeDepartmentID)).count()
        if intRowCount > 0:
            form.errors.fldOeeDepartmentID = 'Duplicate record detected'
    elif 'fldOeeArticleDescription' in form.vars:
        intRowCount = 0
        intRowCount = dboee((dboee.tblOee_Article.fldOeeCountryID == form.vars.fldOeeCountryID) & \
                            (dboee.tblOee_Article.fldOeePlantID == form.vars.fldOeePlantID) & \
                            (dboee.tblOee_Article.fldOeeSubPlantID == form.vars.fldOeeSubPlantID) & \
                            (dboee.tblOee_Article.fldOeeDepartmentID == form.vars.fldOeeDepartmentID) & \
                            (dboee.tblOee_Article.fldOeeArticleNr == form.vars.fldOeeArticleNr)).count()
        if intRowCount > 0:
            form.errors.fldOeeDepartmentID = 'Duplicate record detected'

@auth.requires_login()
def machselect():
    #wanneer weergeven als geen reginfo?
    strIkke = ''
    intScreenWidth = 0
    gintCountryNr = 0
    gintPlantNr = 0
    gintSubPlantNr = 0
    gintDepartmentNr = 0
    formtable = SQLFORM.factory()
    intFromSeconds = 0
    intToSeconds = 0
    try:
        intScreenWidth = int(request.vars['screenwidth'])
        gintCountryNr = int(request.vars['countrynr'])
        gintPlantNr = int(request.vars['plantnr'])
        gintSubPlantNr = int(request.vars['subplantnr'])
        gintDepartmentNr = int(request.vars['departmentnr'])
    except:
        pass
    if gintCountryNr == 0:
        redirect(URL('machselect', vars=dict(countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    if gintPlantNr == 0:
        redirect(URL('machselect', vars=dict(countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    if gintSubPlantNr == 0:
        redirect(URL('machselect', vars=dict(countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    if gintDepartmentNr == 0:
        redirect(URL('machselect', vars=dict(countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    strCountry = dboee(dboee.tblOee_Country.fldOeeCountryNr == gintCountryNr).select()[0].get('fldOeeCountryDescription')
    strPlant = dboee((dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr) & \
                     (dboee.tblOee_Plant.fldOeePlantNr == gintPlantNr)).select()[0].get('fldOeePlantDescription')
    strSubPlant = dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                        (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr) & \
                        (dboee.tblOee_SubPlant.fldOeeSubPlantNr == gintSubPlantNr)).select()[0].get('fldOeeSubPlantDescription')
    strDepartment = dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                          (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                          (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr) & \
                          (dboee.tblOee_Department.fldOeeDepartmentNr == gintDepartmentNr)).select()[0].get('fldOeeDepartmentDescription')
    #all machines
    arrMachinesSel = dboee((dboee.tblOee_Machine.fldOeeCountryID == gintCountryNr) & \
                           (dboee.tblOee_Machine.fldOeePlantID == gintPlantNr) & \
                           (dboee.tblOee_Machine.fldOeeSubPlantID == gintSubPlantNr) & \
                           (dboee.tblOee_Machine.fldOeeDepartmentID == gintDepartmentNr)).select(dboee.tblOee_Machine.fldOeeMachineNr, \
                                                                                                 dboee.tblOee_Machine.fldOeeMachineCode, \
                                                                                                 dboee.tblOee_Machine.fldOeeMachineDescription, \
                                                                                                 dboee.tblOee_Machine.fldOeeMachineInformation)
    intX = 0
    arrMachines = []
    for arrMachineSel in arrMachinesSel:
        arrMachines.append([])
        try:
            #get last shift startdate
            datToday = date.today()
            rowdaily = dboee((dboee.tblOee_DailySchedule.fldOeeCountryID == gintCountryNr) & \
                             (dboee.tblOee_DailySchedule.fldOeePlantID == gintPlantNr) & \
                             (dboee.tblOee_DailySchedule.fldOeeSubPlantID == gintSubPlantNr) & \
                             (dboee.tblOee_DailySchedule.fldOeeDepartmentID == gintDepartmentNr) & \
                             (dboee.tblOee_DailySchedule.fldOeeDailyScheduleStartDate >= \
                                                         date(datToday.year, datToday.month, datToday.day))).select(dboee.tblOee_DailySchedule.ALL, \
                                                                               orderby=~dboee.tblOee_DailySchedule.fldOeeDailyScheduleNr, \
                                                                               limitby=(0,3))
            rowshift = dboee((dboee.tblOee_ShiftTime.fldOeeCountryID == gintCountryNr) & \
                             (dboee.tblOee_ShiftTime.fldOeePlantID == gintPlantNr) & \
                             (dboee.tblOee_ShiftTime.fldOeeSubPlantID == gintSubPlantNr) & \
                             (dboee.tblOee_ShiftTime.fldOeeDepartmentID == gintDepartmentNr)).select(dboee.tblOee_ShiftTime.ALL, \
                                                                               orderby=~dboee.tblOee_ShiftTime.fldOeeShiftTimeNr, \
                                                                               limitby=(0,3))
            for row in rowshift:
                datStartShift = datetime(datToday.year, datToday.month, datToday.day, row.fldOeeShiftTimeStart.hour, row.fldOeeShiftTimeStart.minute)
                intShiftDuration = (row.fldOeeShiftTimeEnd - row.fldOeeShiftTimeStart).total_seconds()
                datEndShift = datStartShift + timedelta(0, intShiftDuration)
                if datStartShift <= datetime.now():
                    if datEndShift >= datetime.now():
                        datDateRange = datStartShift
                        datFrom = datStartShift
                        intFromSeconds = (datStartShift - datetime(1970,1,1)).total_seconds()
                        intToSeconds = (datEndShift - datetime(1970,1,1)).total_seconds()

            #all machines with registered data
            arrMachine = dboee((dboee.tblOee_Machine.fldOeeCountryID == gintCountryNr) & \
                               (dboee.tblOee_Machine.fldOeePlantID == gintPlantNr) & \
                               (dboee.tblOee_Machine.fldOeeSubPlantID == gintSubPlantNr) & \
                               (dboee.tblOee_Machine.fldOeeDepartmentID == gintDepartmentNr) & \
                               (dboee.tblOee_Machine.fldOeeCountryID == dboee.tblOee_Reg.fldOeeCountryID) & \
                               (dboee.tblOee_Machine.fldOeePlantID == dboee.tblOee_Reg.fldOeePlantID) & \
                               (dboee.tblOee_Machine.fldOeeSubPlantID == dboee.tblOee_Reg.fldOeeSubPlantID) & \
                               (dboee.tblOee_Machine.fldOeeDepartmentID == dboee.tblOee_Reg.fldOeeDepartmentID) & \
                               (dboee.tblOee_Machine.fldOeeMachineNr == arrMachinesSel[intX].fldOeeMachineNr) & \
                               (dboee.tblOee_Machine.fldOeeMachineNr == dboee.tblOee_Reg.fldOeeMachineID) & \
                               (dboee.tblOee_Reg.fldOeeStartDateTime > datDateRange)).select(dboee.tblOee_Machine.fldOeeMachineNr, \
                                                                                               dboee.tblOee_Machine.fldOeeMachineCode, \
                                                                                               dboee.tblOee_Machine.fldOeeMachineDescription, \
                                                                                               dboee.tblOee_Machine.fldOeeMachineInformation, \
                                                                                               dboee.tblOee_Reg.fldOeeActivityDescription, \
                                                                                               dboee.tblOee_Reg.fldOeeMachineStatusID, \
                                                                                               dboee.tblOee_Reg.fldOeeActivityGroupDescription, \
                                                                                               orderby=~dboee.tblOee_Reg.fldOeeRegTableKeyID, \
                                                                                               limitby=(0,1))
            arrMachines[intX] = {}
            arrMachines[intX]['MachineNr'] = arrMachineSel.fldOeeMachineNr
            arrMachines[intX]['MachineCode'] = arrMachineSel.fldOeeMachineCode
            arrMachines[intX]['MachineDescription'] = arrMachineSel.fldOeeMachineDescription
            arrMachines[intX]['MachineInformation'] = arrMachineSel.fldOeeMachineInformation
            arrMachines[intX]['MachineStatusNr'] = arrMachine[0].tblOee_Reg.fldOeeMachineStatusID
            arrMachines[intX]['MachineStatusColor'] = machinestatuscolor(arrMachine[0].tblOee_Reg.fldOeeMachineStatusID)
            arrMachines[intX]['ActivityGroup'] = arrMachine[0].tblOee_Reg.fldOeeActivityGroupDescription
            arrMachines[intX]['Activity'] = arrMachine[0].tblOee_Reg.fldOeeActivityDescription
        except:
            arrMachines[intX]['ActivityGroup'] = 'No registered data'
            arrMachines[intX]['Activity'] = 'None'
            arrMachines[intX]['MachineStatusNr'] = '0'
            arrMachines[intX]['MachineStatusColor'] = machinestatuscolor(0)
        finally:
            intX+=1
        datTo = datetime.now()
        formtable = SQLFORM.factory(Field('Select', 'text', \
                                    default = 'Current Shift', \
                                    widget = SQLFORM.widgets.radio.widget, \
                                    requires = [IS_IN_SET(['Current Shift', 'Last 24 hours', 'Custom'], zero = None)]), \
                                    Field('from_date', 'datetime', label = 'From', default = datFrom), \
                                    Field('to_date', 'datetime', label = 'To', default = datTo), \
                                    formstyle='table2cols')
    if formtable.process(formname='formtable').accepted:
        intFromSeconds = (formtable.vars.from_date - datetime(1970,1,1)).total_seconds()
        intToSeconds = (formtable.vars.to_date - datetime(1970,1,1)).total_seconds()
        intRange = 0
        if formtable.vars.Select == "Current Shift":
            intRange = 1
        elif formtable.vars.Select == "Last 24 hours":
            intRange = 2
        elif formtable.vars.Select == "Custom":
            intRange = 3
        redirect(URL('tableview', vars=dict(countrynr=gintCountryNr, plantnr=gintPlantNr, subplantnr=gintSubPlantNr, departmentnr=gintDepartmentNr, range=intRange, fromdate=int(intFromSeconds), todate=int(intToSeconds))))
    formtable.element("input", _type="submit")["_value"] = "View"

    return dict(strLevel = 'Machine', \
                gintCountryNr = gintCountryNr, \
                gintPlantNr = gintPlantNr, \
                gintSubPlantNr = gintSubPlantNr, \
                gintDepartmentNr = gintDepartmentNr, \
                strCountry = strCountry, \
                strPlant = strPlant, \
                strSubPlant = strSubPlant, \
                strDepartment = strDepartment, \
                arrMachines = arrMachines, \
                intScreenWidth = intScreenWidth, \
                intFromSeconds = intFromSeconds, \
                intToSeconds = intToSeconds, \
                formtable = formtable)

@auth.requires_login()
def machdetails():
    gintMachineID = 0
    gintCountryNr = 0
    gintPlantNr = 0
    gintSubPlantNr = 0
    gintDepartmentNr = 0
    datDateRange = (datetime.now() - timedelta(0, 28800))
    datFrom = datetime.now()
    datTo = datetime.now()
    intFromSeconds = 0
    intToSeconds = 0
    intGraph = 0
    try:
        gintMachineID = int(request.vars['machineid'])
        gintCountryNr = int(request.vars['countrynr'])
        gintPlantNr = int(request.vars['plantnr'])
        gintSubPlantNr = int(request.vars['subplantnr'])
        gintDepartmentNr = int(request.vars['departmentnr'])
        intRange = int(request.vars['range'])
    except:
        intRange = 1
    try:
        intGraph = int(request.vars['graph'])
    except:
        intGraph = 0
    if gintMachineID == 0:
        redirect(URL('machdetails', vars=dict(machineid=gintDefMachID, countrynr=gintCountryNr, plantnr=gintPlantNr, subplantnr=gintSubPlantNr, departmentnr=gintDepartmentNr)))
    if gintCountryNr == 0:
        redirect(URL('machdetails', vars=dict(machineid=gintDefMachID, countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    if gintPlantNr == 0:
        redirect(URL('machdetails', vars=dict(machineid=gintDefMachID, countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    if gintSubPlantNr == 0:
        redirect(URL('machdetails', vars=dict(machineid=gintDefMachID, countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    if gintDepartmentNr == 0:
        redirect(URL('machdetails', vars=dict(machineid=gintDefMachID, countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))

    strLevel = "Machine"
    #get OEE info from last record
    try:
        #get data range
        if intRange == 1:
            datToday = date.today()
            rowdaily = dboee((dboee.tblOee_DailySchedule.fldOeeCountryID == gintCountryNr) & \
                             (dboee.tblOee_DailySchedule.fldOeePlantID == gintPlantNr) & \
                             (dboee.tblOee_DailySchedule.fldOeeSubPlantID == gintSubPlantNr) & \
                             (dboee.tblOee_DailySchedule.fldOeeDepartmentID == gintDepartmentNr) & \
                             (dboee.tblOee_DailySchedule.fldOeeDailyScheduleStartDate >= \
                                                         date(datToday.year, datToday.month, datToday.day))).select(dboee.tblOee_DailySchedule.ALL, \
                                                                               orderby=~dboee.tblOee_DailySchedule.fldOeeDailyScheduleNr, \
                                                                               limitby=(0,3))
            rowshift = dboee((dboee.tblOee_ShiftTime.fldOeeCountryID == gintCountryNr) & \
                             (dboee.tblOee_ShiftTime.fldOeePlantID == gintPlantNr) & \
                             (dboee.tblOee_ShiftTime.fldOeeSubPlantID == gintSubPlantNr) & \
                             (dboee.tblOee_ShiftTime.fldOeeDepartmentID == gintDepartmentNr)).select(dboee.tblOee_ShiftTime.ALL, \
                                                                               orderby=~dboee.tblOee_ShiftTime.fldOeeShiftTimeNr, \
                                                                               limitby=(0,3))
            for row in rowshift:
                datStartShift = datetime(datToday.year, datToday.month, datToday.day, row.fldOeeShiftTimeStart.hour, row.fldOeeShiftTimeStart.minute)
                intShiftDuration = (row.fldOeeShiftTimeEnd - row.fldOeeShiftTimeStart).total_seconds()
                datEndShift = datStartShift + timedelta(0, intShiftDuration)
                if datStartShift <= datetime.now():
                    if datEndShift >= datetime.now():
                        datDateRange = datStartShift
                        datFrom = datStartShift
                        intFromSeconds = (datStartShift - datetime(1970,1,1)).total_seconds()
                        intToSeconds = (datEndShift - datetime(1970,1,1)).total_seconds()
            row = dboee((dboee.tblOee_Reg.fldOeeMachineID == gintMachineID) & \
                        (dboee.tblOee_Reg.fldOeeCountryID == gintCountryNr) & \
                        (dboee.tblOee_Reg.fldOeePlantID == gintPlantNr) & \
                        (dboee.tblOee_Reg.fldOeeSubPlantID == gintSubPlantNr) & \
                        (dboee.tblOee_Reg.fldOeeDepartmentID == gintDepartmentNr) & \
                        (dboee.tblOee_Reg.fldOeeStartDateTime > datDateRange)).select(dboee.tblOee_Reg.ALL, \
                                                                                      orderby=~dboee.tblOee_Reg.fldOeeRegNr, \
                                                                                      limitby=(0,1))
        elif intRange == 2:
            datDateRange = (datetime.now() - timedelta(0, 86400))
            datFrom = (datetime.now() - timedelta(0, 86400))
            intFromSeconds = (datDateRange - datetime(1970,1,1)).total_seconds()
            intToSeconds = (datetime.now() - datetime(1970,1,1)).total_seconds()
            row = dboee((dboee.tblOee_Reg.fldOeeMachineID == gintMachineID) & \
                        (dboee.tblOee_Reg.fldOeeCountryID == gintCountryNr) & \
                        (dboee.tblOee_Reg.fldOeePlantID == gintPlantNr) & \
                        (dboee.tblOee_Reg.fldOeeSubPlantID == gintSubPlantNr) & \
                        (dboee.tblOee_Reg.fldOeeDepartmentID == gintDepartmentNr) & \
                        (dboee.tblOee_Reg.fldOeeStartDateTime > datDateRange)).select(dboee.tblOee_Reg.ALL, \
                                                                                      orderby=~dboee.tblOee_Reg.fldOeeRegNr, \
                                                                                      limitby=(0,1))
        elif intRange == 3:
            datFrom = datetime.now()
            datTo = datetime.now()
            try:
                intFrom = int(request.vars['fromdate'])
                intTo = int(request.vars['todate'])
                datFrom = (datetime(1970,1,1) + timedelta(0, intFrom))
                datTo = (datetime(1970,1,1) + timedelta(0, intTo))
                intFromSeconds = (datFrom - datetime(1970,1,1)).total_seconds()
                intToSeconds = (datTo - datetime(1970,1,1)).total_seconds()
            except:
                pass
            row = dboee((dboee.tblOee_Reg.fldOeeMachineID == gintMachineID) & \
                        (dboee.tblOee_Reg.fldOeeCountryID == gintCountryNr) & \
                        (dboee.tblOee_Reg.fldOeePlantID == gintPlantNr) & \
                        (dboee.tblOee_Reg.fldOeeSubPlantID == gintSubPlantNr) & \
                        (dboee.tblOee_Reg.fldOeeDepartmentID == gintDepartmentNr) & \
                        (dboee.tblOee_Reg.fldOeeStartDateTime > datFrom) & \
                        (dboee.tblOee_Reg.fldOeeEndDateTime < datTo)).select(dboee.tblOee_Reg.ALL, \
                                                                                      orderby=~dboee.tblOee_Reg.fldOeeRegNr, \
                                                                                      limitby=(0,1))
        intRegNr = row[0].fldOeeRegNr
        strCountry = row[0].fldOeeCountryDescription
        strPlant = row[0].fldOeePlantDescription
        strSubPlant = row[0].fldOeeSubPlantDescription
        strDepartment = row[0].fldOeeDepartmentDescription
        intMachineCode = row[0].fldOeeMachineCode
        strMachine = row[0].fldOeeMachineDescription
        strTeam = row[0].fldOeeTeamDescription
        if strTeam == '':
            strTeam = 'Not set'
        strShiftTime = row[0].fldOeeShiftTimeDescription
        if strShiftTime == '':
            strShiftTime = 'Not set'
        intOEE = row[0].fldOeeCurrentOee
        intPerformance = row[0].fldOeeCurrentPerformance
        intAvailability = row[0].fldOeeCurrentAvailability
        intQuality = row[0].fldOeeCurrentQuality
        intSpeed = row[0].fldOeeAverageSpeed
        intNormspeed = row[0].fldOeeNormSpeed
        intCounter = row[0].fldOeeCounter
        strCounterUnit = row[0].fldOeeCounterUnitDescription
        strActivityGroup = row[0].fldOeeActivityGroupDescription
        strActivity = row[0].fldOeeActivityDescription
        strMachineStatusColor = machinestatuscolor(row[0].fldOeeMachineStatusID)
        datTime = row[0].fldOeeEndDateTime
    except IndexError as strError:
        arrMachinesSel = dboee((dboee.tblOee_Machine.fldOeeCountryID == gintCountryNr) & \
                               (dboee.tblOee_Machine.fldOeePlantID == gintPlantNr) & \
                               (dboee.tblOee_Machine.fldOeeSubPlantID == gintSubPlantNr) & \
                               (dboee.tblOee_Machine.fldOeeDepartmentID == gintDepartmentNr) & \
                               (dboee.tblOee_Machine.fldOeeMachineNr == gintMachineID) & \
                               (dboee.tblOee_Machine.fldOeeCountryID == dboee.tblOee_Country.fldOeeCountryNr) & \
                               (dboee.tblOee_Machine.fldOeePlantID == dboee.tblOee_Plant.fldOeePlantNr) & \
                               (dboee.tblOee_Machine.fldOeeSubPlantID == dboee.tblOee_SubPlant.fldOeeSubPlantNr) & \
                               (dboee.tblOee_Machine.fldOeeDepartmentID == \
                                dboee.tblOee_Department.fldOeeDepartmentNr)).select(dboee.tblOee_Machine.ALL, \
                                                                                    dboee.tblOee_Country.fldOeeCountryDescription, \
                                                                                    dboee.tblOee_Plant.fldOeePlantDescription, \
                                                                                    dboee.tblOee_SubPlant.fldOeeSubPlantDescription, \
                                                                                    dboee.tblOee_Department.fldOeeDepartmentDescription)
        intRegNr = 0
        strCountry = arrMachinesSel[0].tblOee_Country.fldOeeCountryDescription
        strPlant = arrMachinesSel[0].tblOee_Plant.fldOeePlantDescription
        strSubPlant = arrMachinesSel[0].tblOee_SubPlant.fldOeeSubPlantDescription
        strDepartment = arrMachinesSel[0].tblOee_Department.fldOeeDepartmentDescription
        intMachineCode = arrMachinesSel[0].tblOee_Machine.fldOeeMachineCode
        strMachine = arrMachinesSel[0].tblOee_Machine.fldOeeMachineDescription
        strTeam = 'not defined'
        strShiftTime = 'not defined'
        intOEE = 100
        intPerformance = 100
        intAvailability = 100
        intQuality = 100
        intSpeed = 0
        intNormspeed = 0
        intCounter = 0
        strCounterUnit = 'meters'
        strActivityGroup = 'No registered data'
        strActivity = 'None'
        strMachineStatusColor = machinestatuscolor(0)
        datTime = 'No registered data'
    #empty datefield, when not needed
    if intRange == 1:
        datFrom = None
        datTo = None
    elif intRange == 2:
        datFrom = None
        datTo = None
    form = SQLFORM.factory(Field('Select', 'text', \
                                 default = 'Current Shift', \
                                 widget = SQLFORM.widgets.radio.widget, \
                                 requires = [IS_IN_SET(['Current Shift', 'Last 24 hours', 'Custom'], zero = None)]), \
                           Field('from_date', 'datetime', label = 'From', default = datFrom), \
                           Field('to_date', 'datetime', label = 'To', default = datTo))
    if form.accepts(request,session):
        if intRange == 3:
            intFromSeconds = (form.vars.from_date - datetime(1970,1,1)).total_seconds()
            intToSeconds = (form.vars.to_date - datetime(1970,1,1)).total_seconds()
            redirect(URL('machdetails', vars=dict(machineid=gintMachineID, countrynr=gintCountryNr, plantnr=gintPlantNr, subplantnr=gintSubPlantNr, departmentnr=gintDepartmentNr, range=3, fromdate=int(intFromSeconds), todate=int(intToSeconds), graph=intGraph)))
    option1 = form.element("input", _type="radio", _value='Current Shift')
    option1["_onclick"] = "rangeselection(1);"
    option2 = form.element("input", _type="radio", _value="Last 24 hours")
    option2["_onclick"] = "rangeselection(2);"
    option3 = form.element("input", _type="radio", _value="Custom")
    option3["_onclick"] = "rangeselection(3);"
    intScreenWidth = 0
    return dict(form = form, \
                 intRegNr = intRegNr, \
                 gintMachineID = gintMachineID, \
                 strMachine = strMachine, \
                 gintCountryNr = gintCountryNr, \
                 gintPlantNr = gintPlantNr, \
                 gintSubPlantNr = gintSubPlantNr, \
                 gintDepartmentNr = gintDepartmentNr, \
                 strCountry = strCountry, \
                 strPlant = strPlant, \
                 strSubPlant = strSubPlant, \
                 strDepartment = strDepartment, \
                 intMachineCode = intMachineCode, \
                 strTeam = strTeam, \
                 strShiftTime = strShiftTime, \
                 intOEE = intOEE, \
                 intPerformance = intPerformance, \
                 intAvailability = intAvailability, \
                 intQuality = intQuality, \
                 intSpeed = intSpeed, \
                 intNormspeed = intNormspeed, \
                 intCounter = intCounter, \
                 strCounterUnit = strCounterUnit, \
                 strActivityGroup = strActivityGroup, \
                 strActivity = strActivity, \
                 strMachineStatusColor = strMachineStatusColor, \
                 datTime = datTime, \
                 datDateRange = datDateRange, \
                 datFrom = datFrom, \
                 datTo = datTo, \
                 intFromSeconds = intFromSeconds, \
                 intToSeconds = intToSeconds, \
                 strLevel = strLevel,
                 intScreenWidth = intScreenWidth, \
                 intRange = intRange, \
                 intGraph = intGraph, \
                 formtable = "")

def machinestatuscolor(intStatusNr):
    if intStatusNr == 0:
        return str('#E7F3FF')
    elif intStatusNr == 1:
        return str('#FFC2FF')
    elif intStatusNr == 2:
        return str('#CCEBFF')
    elif intStatusNr == 3:
        return str('#B2FF99')
    elif intStatusNr == 4:
        return str('#FFFF80')
    elif intStatusNr == 5:
        return str('#FFD280')
    elif intStatusNr == 6:
        return str('#FFAD99')
    elif intStatusNr == 7:
        return str('#E7F3FF')

def oeegraph():
    gf = Graphs()
    return gf.oeegraph()

def tfhoursbar():
    gf = Graphs()
    return gf.tfhoursbar()

def activitygraph():
    gf = Graphs()
    return gf.activitygraph()

def oeeprogress():
    gf = Graphs()
    return gf.oeeprogress()

@auth.requires_login()
def cockpit():
    intCountryNr = 0
    intPlantNr = 0
    intSubPlantNr = 0
    intDepartmentNr = 0
    intMachineID = 0
    datDateRange = datetime.now()
    intFromSeconds = 0
    intToSeconds = 0
    datFrom = datetime.now()
    datTo = datetime.now()
    try:
        intCountryNr = int(request.vars['countrynr'])
        intPlantNr = int(request.vars['plantnr'])
        intSubPlantNr = int(request.vars['subplantnr'])
        intDepartmentNr = int(request.vars['departmentnr'])
    except:
        pass
    try:
        intFrom = int(request.vars['fromdate'])
        intTo = int(request.vars['todate'])
        datFrom = (datetime(1970,1,1) + timedelta(0, intFrom))
        datTo = (datetime(1970,1,1) + timedelta(0, intTo))
        intFromSeconds = (datFrom - datetime(1970,1,1)).total_seconds()
        intToSeconds = (datTo - datetime(1970,1,1)).total_seconds()
    except:
        datFrom = (datetime.now() - timedelta(days=1))
        datTo = (datetime.now())
    if intCountryNr == 0:
        redirect(URL('cockpit', vars=dict(countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    if intPlantNr == 0:
        redirect(URL('cockpit', vars=dict(countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    if intSubPlantNr == 0:
        redirect(URL('cockpit', vars=dict(countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    if intDepartmentNr == 0:
        redirect(URL('cockpit', vars=dict(countrynr=1, plantnr=1, subplantnr=1, departmentnr=1)))
    strCountry = dboee(dboee.tblOee_Country.fldOeeCountryNr == intCountryNr).select()[0].get('fldOeeCountryDescription')
    strPlant = dboee((dboee.tblOee_Plant.fldOeeCountryID == intCountryNr) & \
                     (dboee.tblOee_Plant.fldOeePlantNr == intPlantNr)).select()[0].get('fldOeePlantDescription')
    strSubPlant = dboee((dboee.tblOee_SubPlant.fldOeeCountryID == intCountryNr) & \
                        (dboee.tblOee_SubPlant.fldOeePlantID == intPlantNr) & \
                        (dboee.tblOee_SubPlant.fldOeeSubPlantNr == intSubPlantNr)).select()[0].get('fldOeeSubPlantDescription')
    strDepartment = dboee((dboee.tblOee_Department.fldOeeCountryID == intCountryNr) & \
                          (dboee.tblOee_Department.fldOeePlantID == intPlantNr) & \
                          (dboee.tblOee_Department.fldOeeSubPlantID == intSubPlantNr) & \
                          (dboee.tblOee_Department.fldOeeDepartmentNr == intDepartmentNr)).select()[0].get('fldOeeDepartmentDescription')
    datDateRange = (datetime.now() - timedelta(0, 86400))
    #all machines
    arrMachinesSel = dboee((dboee.tblOee_Machine.fldOeeCountryID == intCountryNr) & \
                           (dboee.tblOee_Machine.fldOeePlantID == intPlantNr) & \
                           (dboee.tblOee_Machine.fldOeeSubPlantID == intSubPlantNr) & \
                           (dboee.tblOee_Machine.fldOeeDepartmentID == intDepartmentNr)).select(dboee.tblOee_Machine.fldOeeMachineNr, \
                                                                                                 dboee.tblOee_Machine.fldOeeMachineCode, \
                                                                                                 dboee.tblOee_Machine.fldOeeMachineDescription, \
                                                                                                 dboee.tblOee_Machine.fldOeeMachineInformation)
    intX = 0
    arrMachines = []
    for arrMachineSel in arrMachinesSel:
        arrMachines.append([])
        try:
            #all machines with registered data
            arrMachine = dboee((dboee.tblOee_Machine.fldOeeCountryID == intCountryNr) & \
                               (dboee.tblOee_Machine.fldOeePlantID == intPlantNr) & \
                               (dboee.tblOee_Machine.fldOeeSubPlantID == intSubPlantNr) & \
                               (dboee.tblOee_Machine.fldOeeDepartmentID == intDepartmentNr) & \
                               (dboee.tblOee_Reg.fldOeeStartDateTime > datDateRange) & \
                               (dboee.tblOee_Machine.fldOeeCountryID == dboee.tblOee_Reg.fldOeeCountryID) & \
                               (dboee.tblOee_Machine.fldOeePlantID == dboee.tblOee_Reg.fldOeePlantID) & \
                               (dboee.tblOee_Machine.fldOeeSubPlantID == dboee.tblOee_Reg.fldOeeSubPlantID) & \
                               (dboee.tblOee_Machine.fldOeeDepartmentID == dboee.tblOee_Reg.fldOeeDepartmentID) & \
                               (dboee.tblOee_Machine.fldOeeMachineNr == arrMachinesSel[intX].fldOeeMachineNr) &\
                               (dboee.tblOee_Machine.fldOeeMachineNr == dboee.tblOee_Reg.fldOeeMachineID)).select(dboee.tblOee_Machine.ALL, \
                                                                                               dboee.tblOee_Reg.ALL, \
                                                                                               orderby=~dboee.tblOee_Reg.fldOeeRegTableKeyID, \
                                                                                               limitby=(0,1))
            arrMachines[intX] = {}
            arrMachines[intX]['MachineID'] = arrMachineSel.fldOeeMachineNr
            arrMachines[intX]['MachineCode'] = arrMachineSel.fldOeeMachineCode
            arrMachines[intX]['MachineDescription'] = arrMachineSel.fldOeeMachineDescription
            arrMachines[intX]['MachineInformation'] = arrMachineSel.fldOeeMachineInformation
            arrMachines[intX]['MachineStatusNr'] = arrMachine[0].tblOee_Reg.fldOeeMachineStatusID
            arrMachines[intX]['MachineStatusColor'] = machinestatuscolor(arrMachine[0].tblOee_Reg.fldOeeMachineStatusID)
            arrMachines[intX]['ActivityGroup'] = arrMachine[0].tblOee_Reg.fldOeeActivityGroupDescription
            arrMachines[intX]['Activity'] = arrMachine[0].tblOee_Reg.fldOeeActivityDescription
            arrMachines[intX]['Availability'] = arrMachine[0].tblOee_Reg.fldOeeCurrentAvailability
            arrMachines[intX]['Performance'] = arrMachine[0].tblOee_Reg.fldOeeCurrentPerformance
            arrMachines[intX]['Quality'] = arrMachine[0].tblOee_Reg.fldOeeCurrentQuality
            arrMachines[intX]['OEE'] = arrMachine[0].tblOee_Reg.fldOeeCurrentOee
        except:
            arrMachines[intX]['ActivityGroup'] = 'No registered data'
            arrMachines[intX]['Activity'] = 'None'
            arrMachines[intX]['MachineStatusNr'] = '0'
            arrMachines[intX]['MachineStatusColor'] = machinestatuscolor(0)
            arrMachines[intX]['Availability'] = 100
            arrMachines[intX]['Performance'] = 100
            arrMachines[intX]['Quality'] = 100
            arrMachines[intX]['OEE'] = 100
        finally:
            intX+=1
    form = SQLFORM.factory(Field('from_date', 'datetime', label = 'From', default = datFrom), \
                           Field('to_date', 'datetime', label = 'To', default = datTo), \
                           formstyle='table2cols')
    if form.process(formname='form').accepted:
        intFromSeconds = (form.vars.from_date - datetime(1970,1,1)).total_seconds()
        intToSeconds = (form.vars.to_date - datetime(1970,1,1)).total_seconds()
        redirect(URL('cockpit', vars=dict(countrynr=intCountryNr, plantnr=intPlantNr, subplantnr=intSubPlantNr, departmentnr=intDepartmentNr, fromdate=int(intFromSeconds), todate=int(intToSeconds))))
    form.element("input", _type="submit")["_value"] = "Update"
    formtable = SQLFORM.factory(Field('Select', 'text', \
                                 default = 'Current Shift', \
                                 widget = SQLFORM.widgets.radio.widget, \
                                 requires = [IS_IN_SET(['Current Shift', 'Last 24 hours', 'Custom'], zero = None)]), \
                                 formstyle='table2cols')
    if formtable.process(formname='formtable').accepted:
        intRange = 0
        if formtable.vars.Select == "Current Shift":
            intRange = 1
        elif formtable.vars.Select == "Last 24 hours":
            intRange = 2
        elif formtable.vars.Select == "Custom":
            intRange = 3
        redirect(URL('tableview', vars=dict(countrynr=intCountryNr, plantnr=intPlantNr, subplantnr=intSubPlantNr, departmentnr=intDepartmentNr, range=intRange, fromdate=int(intFromSeconds), todate=int(intToSeconds))))
    formtable.element("input", _type="submit")["_value"] = "View"
    return dict(gintCountryNr = intCountryNr, \
                gintPlantNr = intPlantNr, \
                gintSubPlantNr = intSubPlantNr, \
                gintDepartmentNr = intDepartmentNr, \
                strCountry = strCountry, \
                strPlant = strPlant, \
                strSubPlant = strSubPlant, \
                strDepartment = strDepartment, \
                arrMachines = arrMachines, \
                strLevel = 'Machine', \
                intScreenWidth = 1066, \
                intFromSeconds = intFromSeconds, \
                intToSeconds = intToSeconds, \
                form = form, \
                formtable = formtable)

@auth.requires_login()
def tableview():
    try:
        gintCountryNr = int(request.vars['countrynr'])
        gintPlantNr = int(request.vars['plantnr'])
        gintSubPlantNr = int(request.vars['subplantnr'])
        gintDepartmentNr = int(request.vars['departmentnr'])
        intRange = int(request.vars['range'])
        intFrom = int(request.vars['fromdate'])
        intTo = int(request.vars['todate'])
        datFrom = (datetime(1970,1,1) + timedelta(0, intFrom))
        datTo = (datetime(1970,1,1) + timedelta(0, intTo))
    except:
        pass
    if intRange == 1:
        datToday = date.today()
        rowdaily = dboee((dboee.tblOee_DailySchedule.fldOeeCountryID == gintCountryNr) & \
                         (dboee.tblOee_DailySchedule.fldOeePlantID == gintPlantNr) & \
                         (dboee.tblOee_DailySchedule.fldOeeSubPlantID == gintSubPlantNr) & \
                         (dboee.tblOee_DailySchedule.fldOeeDepartmentID == gintDepartmentNr) & \
                         (dboee.tblOee_DailySchedule.fldOeeDailyScheduleStartDate >= \
                                                     date(datToday.year, datToday.month, datToday.day))).select(dboee.tblOee_DailySchedule.ALL, \
                                                                           orderby=~dboee.tblOee_DailySchedule.fldOeeDailyScheduleNr, \
                                                                           limitby=(0,3))
        rowshift = dboee((dboee.tblOee_ShiftTime.fldOeeCountryID == gintCountryNr) & \
                         (dboee.tblOee_ShiftTime.fldOeePlantID == gintPlantNr) & \
                         (dboee.tblOee_ShiftTime.fldOeeSubPlantID == gintSubPlantNr) & \
                         (dboee.tblOee_ShiftTime.fldOeeDepartmentID == gintDepartmentNr)).select(dboee.tblOee_ShiftTime.ALL, \
                                                                           orderby=~dboee.tblOee_ShiftTime.fldOeeShiftTimeNr, \
                                                                           limitby=(0,3))
        for row in rowshift:
            datStartShift = datetime(datToday.year, datToday.month, datToday.day, row.fldOeeShiftTimeStart.hour, row.fldOeeShiftTimeStart.minute)
            intShiftDuration = (row.fldOeeShiftTimeEnd - row.fldOeeShiftTimeStart).total_seconds()
            datEndShift = datStartShift + timedelta(0, intShiftDuration)
            if datStartShift <= datetime.now():
                if datEndShift >= datetime.now():
                    datDateRange = datStartShift
                    datFrom = datStartShift
                    datTo = datetime.now()
                    intFromSeconds = (datStartShift - datetime(1970,1,1)).total_seconds()
                    intToSeconds = (datEndShift - datetime(1970,1,1)).total_seconds()
        query = ((dboee.tblOee_Reg.fldOeeCountryID == gintCountryNr) & \
                 (dboee.tblOee_Reg.fldOeePlantID == gintPlantNr) & \
                 (dboee.tblOee_Reg.fldOeeSubPlantID == gintSubPlantNr) & \
                 (dboee.tblOee_Reg.fldOeeDepartmentID == gintDepartmentNr) & \
                 (dboee.tblOee_Reg.fldOeeStartDateTime > datDateRange))
    elif intRange == 2:
        datDateRange = (datetime.now() - timedelta(0, 86400))
        datFrom = (datetime.now() - timedelta(0, 86400))
        datTo = datetime.now()
        intFromSeconds = (datDateRange - datetime(1970,1,1)).total_seconds()
        intToSeconds = (datetime.now() - datetime(1970,1,1)).total_seconds()
        query = ((dboee.tblOee_Reg.fldOeeCountryID == gintCountryNr) & \
                 (dboee.tblOee_Reg.fldOeePlantID == gintPlantNr) & \
                 (dboee.tblOee_Reg.fldOeeSubPlantID == gintSubPlantNr) & \
                 (dboee.tblOee_Reg.fldOeeDepartmentID == gintDepartmentNr) & \
                 (dboee.tblOee_Reg.fldOeeStartDateTime > datDateRange))
    elif intRange == 3:
        datFrom = datetime.now()
        datTo = datetime.now()
        try:
            intFrom = int(request.vars['fromdate'])
            intTo = int(request.vars['todate'])
            datFrom = (datetime(1970,1,1) + timedelta(0, intFrom))
            datTo = (datetime(1970,1,1) + timedelta(0, intTo))
            intFromSeconds = (datFrom - datetime(1970,1,1)).total_seconds()
            intToSeconds = (datTo - datetime(1970,1,1)).total_seconds()
        except:
            pass
        query = ((dboee.tblOee_Reg.fldOeeCountryID == gintCountryNr) & \
                 (dboee.tblOee_Reg.fldOeePlantID == gintPlantNr) & \
                 (dboee.tblOee_Reg.fldOeeSubPlantID == gintSubPlantNr) & \
                 (dboee.tblOee_Reg.fldOeeDepartmentID == gintDepartmentNr) & \
                 (dboee.tblOee_Reg.fldOeeStartDateTime > datFrom) & \
                 (dboee.tblOee_Reg.fldOeeEndDateTime < datTo))
    fields = (dboee.tblOee_Reg.fldOeeMachineCode,
              dboee.tblOee_Reg.fldOeeMachineDescription,
              dboee.tblOee_Reg.fldOeeStartDateTime,
              dboee.tblOee_Reg.fldOeeEndDateTime,
              dboee.tblOee_Reg.fldOeeActivityDuration,
              dboee.tblOee_Reg.fldOeeCurrentPerformance,
              dboee.tblOee_Reg.fldOeeCurrentAvailability,
              dboee.tblOee_Reg.fldOeeCurrentQuality,
              dboee.tblOee_Reg.fldOeeCurrentOee,
              dboee.tblOee_Reg.fldOeeActivityGroupCalcForOee,
              dboee.tblOee_Reg.fldOeeActivityGroupDescription,
              dboee.tblOee_Reg.fldOeeActivityDescription,
              dboee.tblOee_Reg.fldOeeActivityGroupID,
              dboee.tblOee_Reg.fldOeeActivityID,
              dboee.tblOee_Reg.fldOeeMachineStatusID,
              dboee.tblOee_Reg.fldOeeMachineStatusDescription,
              dboee.tblOee_Reg.fldOeeArticleNr,
              dboee.tblOee_Reg.fldOeeArticleDescription,
              dboee.tblOee_Reg.fldOeeOrderNr,
              dboee.tblOee_Reg.fldOeeOrderDescription,
              dboee.tblOee_Reg.fldOeeAverageSpeed,
              dboee.tblOee_Reg.fldOeeNormSpeed,
              dboee.tblOee_Reg.fldOeeCounter,
              dboee.tblOee_Reg.fldOeeCounterUnitID,
              dboee.tblOee_Reg.fldOeeCounterUnitDescription,
              dboee.tblOee_Reg.fldOeeRegNr, \
              dboee.tblOee_Reg.fldOeeMachineID,
              dboee.tblOee_Reg.fldOeeCountryID, \
              dboee.tblOee_Reg.fldOeeCountryDescription,
              dboee.tblOee_Reg.fldOeePlantID, \
              dboee.tblOee_Reg.fldOeePlantDescription,
              dboee.tblOee_Reg.fldOeeSubPlantID, \
              dboee.tblOee_Reg.fldOeeSubPlantDescription,
              dboee.tblOee_Reg.fldOeeDepartmentID,
              dboee.tblOee_Reg.fldOeeDepartmentDescription,
              dboee.tblOee_Reg.fldOeePlantDescription,
              dboee.tblOee_Reg.fldOeeTeamID,
              dboee.tblOee_Reg.fldOeeTeamDescription,
              dboee.tblOee_Reg.fldOeeTeamColorID,
              dboee.tblOee_Reg.fldOeeTeamColorDescription,
              dboee.tblOee_Reg.fldOeeShiftTimeID,
              dboee.tblOee_Reg.fldOeeShiftTimeDescription,
              dboee.tblOee_Reg.fldOeeShiftStartDateTime,
              dboee.tblOee_Reg.fldOeeShiftEndDateTime,
              dboee.tblOee_Reg.fldOeeShiftDuration,
              dboee.tblOee_Reg.fldOeeUserLogInformation,
              dboee.tblOee_Reg.fldOeeUserShiftLogInformation,
              dboee.tblOee_Reg.fldOeeDateModified,
              dboee.tblOee_Reg.fldOeeSyncDate,
              dboee.tblOee_Reg.fldOeeDateModified,
              dboee.tblOee_Reg.fldOeeSync)
    headers = {'tblOee_Reg.fldOeeCountryID':   'Country ID',
               'tblOee_Reg.fldOeePlantID': 'Plant ID',
               'tblOee_Reg.fldOeeSubPlantID': 'Subplant ID',
               'tblOee_Reg.fldOeeDepartmentID': 'Department ID'}
    export_classes = dict(csv=False, json=False, html=False,
                          tsv=True, xml=False, csv_with_hidden_cols=False,
                          tsv_with_hidden_cols=False)
    grid = SQLFORM.grid(query=query,
                        fields=fields,
                        headers=headers,
                        orderby='tblOee_Reg.fldOeeRegTableKeyID DESC',
                        deletable=False,  
                        editable=False,
                        create=False,
                        maxtextlength=40, maxtextlengths={'View_Logbook.Logbook' : 400}, paginate=25, details=True, searchable=True, buttons_placement = 'left')

    strCountry = dboee(dboee.tblOee_Country.fldOeeCountryNr == gintCountryNr).select()[0].get('fldOeeCountryDescription')
    strPlant = dboee((dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr) & \
                     (dboee.tblOee_Plant.fldOeePlantNr == gintPlantNr)).select()[0].get('fldOeePlantDescription')
    strSubPlant = dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                        (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr) & \
                        (dboee.tblOee_SubPlant.fldOeeSubPlantNr == gintSubPlantNr)).select()[0].get('fldOeeSubPlantDescription')
    strDepartment = dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                           (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                           (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr) & \
                           (dboee.tblOee_Department.fldOeeDepartmentNr == gintDepartmentNr)).select()[0].get('fldOeeDepartmentDescription')
    strLevel = 'Machine'

    return dict(gintCountryNr = gintCountryNr, \
                gintPlantNr = gintPlantNr, \
                gintSubPlantNr = gintSubPlantNr, \
                gintDepartmentNr = gintDepartmentNr, \
                strLevel = strLevel, \
                strCountry = strCountry, \
                strPlant = strPlant, \
                strSubPlant = strSubPlant, \
                strDepartment = strDepartment, \
                intScreenWidth = 0, \
                grid = grid, \
                datFrom = datFrom, \
                datTo = datTo)

def graphcolor(intColor):
    if intColor == 1:
        strColor = 'blue'
    elif intColor == 2:
        strColor = 'yellow'
    elif intColor == 3:
        strColor = 'red'
    elif intColor == 4:
        strColor = 'green'
    return strColor

@auth.requires_membership('Administrator')
def oeeadmin():
    intView = 0
    try:
        intView = int(request.vars['view'])
    except:
        intView = 0
    response.menu = [
        (T('Admin Home'), False, URL('oeeadmin')),
        (T('Geographical settings'), False, '#', [
            (T('Countries'), False, URL('oeeadmin', vars=dict(view=1))),
            (T('Plants'), False, URL('oeeadmin', vars=dict(view=2))),
            (T('Sub-Plant'), False, URL('oeeadmin', vars=dict(view=3))),
            (T('Department'), False, URL('oeeadmin', vars=dict(view=4)))]),
        ('Machines and Activities', False, '#', [
            (T('Activitygroups'), False, URL('oeeadmin', vars=dict(view=5))),
            (T('Activities'), False, URL('oeeadmin', vars=dict(view=6))),
            (T('Machine-activities'), False, URL('oeeadmin', vars=dict(view=7))),
            (T('Machines'), False, URL('oeeadmin', vars=dict(view=8))),
            (T('Modules'), False, URL('oeeadmin', vars=dict(view=14))),
            (T('Shortbreaks'), False, URL('oeeadmin', vars=dict(view=9))),
            (T('Undefined Production'), False, URL('oeeadmin', vars=dict(view=10))),
            (T('Undefined Standstill'), False, URL('oeeadmin', vars=dict(view=11))),
            (T('I/O Failures'), False, URL('oeeadmin', vars=dict(view=12))),
            (T('Unscheduled'), False, URL('oeeadmin', vars=dict(view=13)))]),
        (T('Shifts/Teams'), False, None, [
            (T('Shifts'), False, URL('oeeadmin', vars=dict(view=18))),
            (T('Teams'), False, URL('oeeadmin', vars=dict(view=19))),
            (T('Daily schedule'), False, URL('oeeadmin', vars=dict(view=20)))]),
        (T('Articles/Orders'), False, None, [
            (T('Articles'), False, URL('oeeadmin', vars=dict(view=21))),
            (T('Orders'), False, URL('oeeadmin', vars=dict(view=22))),
            (T('Imports/Templates'), False, URL('importwizard'))])
        ]
    
    form_country = ""
    form_plant = ""
    form_subplant = ""
    form_department = ""
    form_activitygroups = ""
    form_activities = ""
    form_machineactivities = ""
    form_machines = ""
    form_modules = ""
    form_moduletypes = ""
    form_machineunits = ""
    form_modulesensorstyle = ""
    form_shortbreaks = ""
    form_undefinedproduction = ""
    form_undefinedstandstill = ""
    form_iofailures = ""
    form_unscheduled = ""
    form_shifts = ""
    form_teams = ""
    form_scheduler = ""
    form_articles = ""
    form_orders = ""
    if intView == 1:
        query=((dboee.tblOee_Country))
        form_country = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, links_in_grid=True, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editcountryform = form_country.element(_class='buttontext button')
                editcountryform[0] = 'Add Country'
                addbutton = form_country.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Country')
    elif intView == 2:
        query=((dboee.tblOee_Plant))
        form_plant = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editplantform = form_plant.element(_class='buttontext button')
                editplantform[0] = 'Add Plant'
                addbutton = form_plant.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Plant')
    elif intView == 3:
        query=((dboee.tblOee_SubPlant))
        form_subplant = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editsubplantform = form_subplant.element(_class='buttontext button')
                editsubplantform[0] = 'Add Sub-Plant'
                addbutton = form_subplant.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=SubPlant')
    elif intView == 4:
        query=((dboee.tblOee_Department))
        form_department = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editdepartmentform = form_department.element(_class='buttontext button')
                editdepartmentform[0] = 'Add Department'
                addbutton = form_department.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Department')
    elif intView == 5:
        query=((dboee.tblOee_ActivityGroup))
        form_activitygroups = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editactivitygroupsform = form_activitygroups.element(_class='buttontext button')
                editactivitygroupsform[0] = 'Add ActivityGroup'
                addbutton = form_activitygroups.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=ActivityGroup')
    elif intView == 6:
        query=((dboee.tblOee_Activity))
        form_activities = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editactivitiesform = form_activities.element(_class='buttontext button')
                editactivitiesform[0] = 'Add Activity'
                addbutton = form_activities.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Activity')
    elif intView == 7:
        query=((dboee.tblOee_MachineActivity))
        form_machineactivities = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editmachineactivitiesform = form_machineactivities.element(_class='buttontext button')
                editmachineactivitiesform[0] = 'Add Machine Activity'
                addbutton = form_machineactivities.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=MachineActivity')
    elif intView == 8:
        query=((dboee.tblOee_Machine))
        form_machines = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editmachinesform = form_machines.element(_class='buttontext button')
                editmachinesform[0] = 'Add Machines'
                addbutton = form_machines.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Machines')
    elif intView == 9:
        query=((dboee.tblOee_MachineShortbreak))
        form_shortbreaks = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editshortbreaksform = form_shortbreaks.element(_class='buttontext button')
                editshortbreaksform[0] = 'Add Shortbreaks'
                addbutton = form_shortbreaks.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Shortbreaks')
    elif intView == 10:
        query=((dboee.tblOee_MachineUndefinedProduction))
        form_undefinedproduction = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editundefinedproductionform = form_undefinedproduction.element(_class='buttontext button')
                editundefinedproductionform[0] = 'Add Undefined Production'
                addbutton = form_undefinedproduction.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=UndefinedProduction')
    elif intView == 11:
        query=((dboee.tblOee_MachineUndefinedStandstill))
        form_undefinedstandstill = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editundefinedstandstillform = form_undefinedstandstill.element(_class='buttontext button')
                editundefinedstandstillform[0] = 'Add Undefined Standstill'
                addbutton = form_undefinedstandstill.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=UndefinedStandstill')
    elif intView == 12:
        query=((dboee.tblOee_MachineIOFailure))
        form_iofailures = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editiofailuresform = form_iofailures.element(_class='buttontext button')
                editiofailuresform[0] = 'Add I/O Failures'
                addbutton = form_iofailures.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=IOFailures')
    elif intView == 13:
        query=((dboee.tblOee_MachineUnscheduled))
        form_unscheduled = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editunscheduledform = form_unscheduled.element(_class='buttontext button')
                editunscheduledform[0] = 'Add Unscheduled'
                addbutton = form_unscheduled.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Unscheduled')
    elif intView == 14:
        query=((dboee.tblOee_Module))
        form_modules = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editmodulesform = form_modules.element(_class='buttontext button')
                editmodulesform[0] = 'Add Modules'
                addbutton = form_modules.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Modules')
    elif intView == 17:
        query=((dboee.tblOee_MachineUnit))
        form_machineunits = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editmachineunitsform = form_machineunits.element(_class='buttontext button')
                editmachineunitsform[0] = 'Add Machine Units'
                addbutton = form_machineunits.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=MachineUnits')
    elif intView == 18:
        query=((dboee.tblOee_ShiftTime))
        form_shifts = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editshiftsform = form_shifts.element(_class='buttontext button')
                editshiftsform[0] = 'Add Shifts'
                addbutton = form_shifts.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Shifts')
    elif intView == 19:
        query=((dboee.tblOee_Team))
        form_teams = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editteamsform = form_teams.element(_class='buttontext button')
                editteamsform[0] = 'Add Teams'
                addbutton = form_teams.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Teams')
    elif intView == 20:
        query=((dboee.tblOee_DailySchedule))
        form_scheduler = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editschedulerform = form_scheduler.element(_class='buttontext button')
                editschedulerform[0] = 'Add Schedule'
                addbutton = form_scheduler.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Scheduler')
    elif intView == 21:
        query=((dboee.tblOee_Article))
        form_articles = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editarticlesform = form_articles.element(_class='buttontext button')
                editarticlesform[0] = 'Add Articles'
                addbutton = form_articles.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Articles')
    elif intView == 22:
        query=((dboee.tblOee_Order))
        form_orders = SQLFORM.grid(query=query, create=True, \
                            searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=False, buttons_placement = 'left')
        if not request.args(0) == 'edit':
            if not request.args(0) == 'view':
                editordersform = form_orders.element(_class='buttontext button')
                editordersform[0] = 'Add Articles'
                addbutton = form_orders.element(_class='button btn btn-default')
                addbutton["_href"] = URL(r=request,f='add?level=Orders')
    return dict(form_country = form_country, \
                form_plant = form_plant, \
                form_subplant = form_subplant, \
                form_department = form_department, \
                form_activitygroups = form_activitygroups, \
                form_activities = form_activities, \
                form_machineactivities = form_machineactivities, \
                form_machines = form_machines, \
                form_modules = form_modules, \
                form_moduletypes = form_moduletypes, \
                form_machineunits = form_machineunits, \
                form_modulesensorstyle = form_modulesensorstyle, \
                form_shortbreaks = form_shortbreaks, \
                form_undefinedproduction = form_undefinedproduction, \
                form_undefinedstandstill = form_undefinedstandstill, \
                form_iofailures = form_iofailures, \
                form_unscheduled = form_unscheduled, \
                form_shifts = form_shifts, \
                form_teams = form_teams, \
                form_scheduler = form_scheduler, \
                form_articles = form_articles, \
                form_orders = form_orders, \
                strLevel = 'Top', \
                intScreenWidth = 1066,\
                intView = intView)

@auth.requires_membership('Administrator')
def importwizard():
    gintCountryNr = 0
    gintPlantNr = 0
    gintSubPlantNr = 0
    gintDepartmentNr = 0
    intArtOrd = 1
    try:
        gintCountryNr = int(request.vars['countrynr'])
        gintPlantNr = int(request.vars['plantnr'])
        gintSubPlantNr = int(request.vars['subplantnr'])
        gintDepartmentNr = int(request.vars['departmentnr'])
        intArtOrd = int(request.vars['artord'])
    except:
        pass
    strLevel = 'Import'
    intScreenWidth = 0
    strArtOrd = ''
    if intArtOrd == 1:
        strArtOrd = 'Articles'
    elif intArtOrd == 2:
        strArtOrd = 'Orders'
    else:
        strArtOrd = 'Articles'

    form = SQLFORM.factory(Field('Select', 'text', \
                                 default = strArtOrd, \
                                 widget = SQLFORM.widgets.radio.widget, \
                                 requires = [IS_IN_SET(['Articles', 'Orders'], zero = None)]), \
                           Field('Country', default = gintCountryNr, requires=IS_IN_DB(dboee, dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')), \
                           Field('Plant', \
                                 default = gintPlantNr, \
                                 requires=IS_IN_DB(dboee(dboee.tblOee_Plant.fldOeeCountryID == gintCountryNr), \
                                 dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')), \
                           Field('SubPlant', \
                                 default = gintSubPlantNr, \
                                 requires=IS_IN_DB(dboee((dboee.tblOee_SubPlant.fldOeeCountryID == gintCountryNr) & \
                                                         (dboee.tblOee_SubPlant.fldOeePlantID == gintPlantNr)), \
                                 dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')), \
                           Field('Department', \
                                 default = gintDepartmentNr, \
                                 requires=IS_IN_DB(dboee((dboee.tblOee_Department.fldOeeCountryID == gintCountryNr) & \
                                                         (dboee.tblOee_Department.fldOeePlantID == gintPlantNr) & \
                                                         (dboee.tblOee_Department.fldOeeSubPlantID == gintSubPlantNr)), \
                                 dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')), \
                           submit_button='Download Template')
    intType = 0
    if form.process(formname='form').accepted:
        try:
            gintCountryNr = form.vars.Country
            gintPlantNr = form.vars.Plant
            gintSubPlantNr = form.vars.SubPlant
            gintDepartmentNr = form.vars.Department
        except:
            pass
        if form.vars.Select == 'Articles':
            intType = 1
        else:
            intType = 2
        redirect(URL('importtemplate.csv', vars=dict(countrynr=gintCountryNr, plantnr=gintPlantNr, subplantnr=gintSubPlantNr, departmentnr=gintDepartmentNr, type=intType)))

    strUploadFolder = 'applications/OEEWeb/uploads'
    formupload = SQLFORM.factory(Field('Select', 'text', \
                                       default = 'Articles', \
                                       widget = SQLFORM.widgets.radio.widget, \
                                       requires = [IS_IN_SET(['Articles', 'Orders'], zero = None)]), \
                                Field('file', 'upload', uploadfolder=strUploadFolder, label='Template',
                                        requires=IS_EMPTY_OR(IS_LENGTH(5242880, 12, error_message=T('File size is not below 5MB')))), \
                                            submit_button='Upload Template')
    if formupload.process(formname='formupload').accepted:
        #strCodedName = formupload.vars.file
        #strOrigName = os.path.basename(request.vars.file.filename)
        #os.rename(os.path.join(strUploadFolder, strCodedName) , os.path.join(strUploadFolder, strOrigName))
        if formupload.vars.Select == 'Articles':
            dboee.tblOee_Article.import_from_csv_file(open(os.path.join(strUploadFolder, formupload.vars.file)))
        else:
            dboee.tblOee_Order.import_from_csv_file(open(os.path.join(strUploadFolder, formupload.vars.file)))

    return dict(strLevel = strLevel, \
                intScreenWidth = intScreenWidth, \
                form = form, \
                gintCountryNr = gintCountryNr, \
                gintPlantNr = gintPlantNr, \
                gintSubPlantNr = gintSubPlantNr, \
                gintDepartmentNr = gintDepartmentNr, \
                intArtOrd = intArtOrd, \
                formupload = formupload)

def importtemplate():
    intCountryNr = int(request.vars['countrynr'])
    intPlantNr = int(request.vars['plantnr'])
    intSubPlantNr = int(request.vars['subplantnr'])
    intDepartmentNr = int(request.vars['departmentnr'])
    intType = int(request.vars['type'])
    if intType == 1:
        rows = dboee((dboee.tblOee_Article.fldOeeCountryID == intCountryNr) & \
                     (dboee.tblOee_Article.fldOeePlantID == intPlantNr) & \
                     (dboee.tblOee_Article.fldOeeSubPlantID == intSubPlantNr) & \
                     (dboee.tblOee_Article.fldOeeDepartmentID == intDepartmentNr)).select(dboee.tblOee_Article.ALL, \
                                                                            limitby=(0,1))
        if bool(rows) == False:
            dboee.tblOee_Article.insert(fldOeeCountryID = intCountryNr, \
                                        fldOeePlantID = intPlantNr, \
                                        fldOeeSubPlantID = intSubPlantNr, \
                                        fldOeeDepartmentID = intDepartmentNr, \
                                        fldOeeArticleNr = '10308H5', \
                                        fldOeeArticleDescription = 'VULT 3G2.5', \
                                        fldOeeArticleNormSpeed = 200, \
                                        fldOeeArticleHistory = False)
            dboee.commit()
            rows = dboee((dboee.tblOee_Article.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Article.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_Article.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_Article.fldOeeDepartmentID == intDepartmentNr)).select(dboee.tblOee_Article.ALL, \
                                                                                limitby=(0,1))
        rows.export_to_csv_file(open('applications/OEEWeb/static/downloads/template.csv', 'wb'))
    elif intType == 2:
        rows = dboee((dboee.tblOee_Order.fldOeeCountryID == intCountryNr) & \
                     (dboee.tblOee_Order.fldOeePlantID == intPlantNr) & \
                     (dboee.tblOee_Order.fldOeeSubPlantID == intSubPlantNr) & \
                     (dboee.tblOee_Order.fldOeeDepartmentID == intDepartmentNr)).select(dboee.tblOee_Order.ALL, \
                                                                          limitby=(0,1))
        if bool(rows) == False:
            dboee.tblOee_Order.insert(fldOeeCountryID = intCountryNr, \
                                      fldOeePlantID = intPlantNr, \
                                      fldOeeSubPlantID = intSubPlantNr, \
                                      fldOeeDepartmentID = intDepartmentNr, \
                                      fldOeeArticleID = '10308H5', \
                                      fldOeeOrderNr = '103377', \
                                      fldOeeOrderDescription = 'VULT 3G2.5', \
                                      fldOeeOrderHistory = False)
            dboee.commit()
            rows = dboee((dboee.tblOee_Article.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Article.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_Article.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_Article.fldOeeDepartmentID == intDepartmentNr)).select(dboee.tblOee_Article.ALL, \
                                                                                limitby=(0,1))
        rows.export_to_csv_file(open('applications/OEEWeb/static/downloads/template.csv', 'wb'))
    return response.stream('applications/OEEWeb/static/downloads/template.csv')

@auth.requires_login()
def addmachines():
    query=((dboee.tblOee_Machine))
    form_machine = SQLFORM.grid(query=query, create=True, exportclasses=dict(csv_with_hidden_cols=False,
                                                                      xml=False,
                                                                      html=False,
                                                                      tsv_with_hidden_cols=False,
                                                                      tsv=False,
                                                                      json=False), \
                        searchable=False, deletable=False, editable=True, maxtextlength=64, csv=True, paginate=25, details=True)
    return dict(form_machine=form_machine, \
                strLevel='Top', \
                intScreenWidth=1066)

def dblocal():
    dbt = DbTools()
    return dbt.dblocal()

@service.run
def api():
    intKindNr = 0
    intMachineID = 0
    intCountryNr = 0
    intPlantNr = 0
    intSubPlantNr = 0
    intDepartmentNr = 0
    intRegFrom = 0
    intRegTo = 0
    intRowCount = 0
    tablename = ()
    strArg = tuple(request.args)
    if strArg[0] == 'get':
        intMachineID = int(request.vars['machineid'])
        intCountryNr = int(request.vars['countrynr'])
        intPlantNr = int(request.vars['plantnr'])
        intSubPlantNr = int(request.vars['subplantnr'])
        intDepartmentNr = int(request.vars['departmentnr'])
        tablename = dboee(dboee.tblOee_Reg.fldOeeCountryID == intCountryNr).select()
    if strArg[0] == 'post':
        intKindNr = int(request.vars['kind'])
        intMachineID = int(request.vars['machineid'])
        intCountryNr = int(request.vars['countrynr'])
        intPlantNr = int(request.vars['plantnr'])
        intSubPlantNr = int(request.vars['subplantnr'])
        intDepartmentNr = int(request.vars['departmentnr'])
        infile = request.vars.file
        filename = infile.filename
        path = os.path.join(request.folder,'private', filename)
        f = open(path, 'wb')
        f.write(infile.file.read())
        f.close()
        if intKindNr == 1:
            dboee.tblOee_Reg.import_from_csv_file(open(path, 'r'))
        elif intKindNr == 2:
            dboee.tblOee_Progress.import_from_csv_file(open(path, 'r'))
            rows = dboee((dboee.tblOee_Progress.fldOeeMachineID == intMachineID) & \
                         (dboee.tblOee_Progress.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Progress.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_Progress.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_Progress.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_Progress.fldOeeSyncDate == None)).select(dboee.tblOee_Progress.ALL)
            #graag alleen not filled ophalen
            for row in rows:
                row.fldOeeSyncDate = datetime.now()
                row.update_record()
    if strArg[0] == 'update':
        pass
    if strArg[0] == 'delete':
        intKindNr = int(request.vars['kind'])
        intMachineID = int(request.vars['machineid'])
        intCountryNr = int(request.vars['countrynr'])
        intPlantNr = int(request.vars['plantnr'])
        intSubPlantNr = int(request.vars['subplantnr'])
        intDepartmentNr = int(request.vars['departmentnr'])
        intRegFrom = int(request.vars['regfrom'])
        intRegTo = int(request.vars['regto'])
        if intKindNr == 1:
            intRowCount = dboee((dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                                (dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                                (dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                                (dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                                (dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                                (dboee.tblOee_Reg.fldOeeRegNr >= intRegFrom) & \
                                (dboee.tblOee_Reg.fldOeeRegNr <= intRegTo)).count()
            if intRowCount > 0:
                dboee((dboee.tblOee_Reg.fldOeeMachineID == intMachineID) & \
                                (dboee.tblOee_Reg.fldOeeCountryID == intCountryNr) & \
                                (dboee.tblOee_Reg.fldOeePlantID == intPlantNr) & \
                                (dboee.tblOee_Reg.fldOeeSubPlantID == intSubPlantNr) & \
                                (dboee.tblOee_Reg.fldOeeDepartmentID == intDepartmentNr) & \
                                (dboee.tblOee_Reg.fldOeeRegNr >= intRegFrom) & \
                                (dboee.tblOee_Reg.fldOeeRegNr <= intRegTo)).delete()
            tablename = dboee(dboee.tblOee_Reg.fldOeeRegTableKeyID < 0).select()
    return dict(tablename = tablename)

def update():
    intUpdate = 0

    intMachineID = 0
    intCountryNr = 0
    intPlantNr = 0
    intSubPlantNr = 0
    intDepartmentNr = 0

    intCountCountry = 0
    intCountPlant = 0
    intCountSubPlant = 0
    intCountDepartment = 0
    intCountActivityGroup = 0
    intCountActivity = 0
    intCountModuleSensorStyle = 0
    intCountModuleType = 0
    intCountModule = 0
    intCountMachineIOFailure = 0
    intCountMachineShortbreak = 0
    intCountMachineStatus = 0
    intCountMachineUndefinedProduction = 0
    intCountMachineUndefinedStandstill = 0
    intCountMachineUnscheduled = 0
    intCountMachineUnit = 0
    intCountMachine = 0
    intCountMachineActivity = 0
    intCountDailySchedule = 0
    intCountArticle = 0
    intCountOrder = 0
    intCountShiftTime = 0
    intCountTeam = 0
    intCountReg = 0
    intCountProgress = 0

    intMachineID = int(request.vars['machineid'])
    intCountryNr = int(request.vars['countrynr'])
    intPlantNr = int(request.vars['plantnr'])
    intSubPlantNr = int(request.vars['subplantnr'])
    intDepartmentNr = int(request.vars['departmentnr'])

    intCountCountry = int(request.vars['countcountry'])
    intCountPlant = int(request.vars['countplant'])
    intCountSubPlant = int(request.vars['countsubplant'])
    intCountDepartment = int(request.vars['countdepartment'])
    intCountActivityGroup = int(request.vars['countactivitygroup'])
    intCountActivity = int(request.vars['countactivity'])
    intCountModuleSensorStyle = int(request.vars['countmodulesensorstyle'])
    intCountModuleType = int(request.vars['countmoduletype'])
    intCountModule = int(request.vars['countmodule'])
    intCountMachineIOFailure = int(request.vars['countmachineiofailure'])
    intCountMachineShortbreak = int(request.vars['countmachineshortbreak'])
    intCountMachineStatus = int(request.vars['countmachinestatus'])
    intCountMachineUndefinedProduction = int(request.vars['countmachineundefinedproduction'])
    intCountMachineUndefinedStandstill = int(request.vars['countmachineundefinedstandstill'])
    intCountMachineUnscheduled = int(request.vars['countmachineunscheduled'])
    intCountMachineUnit = int(request.vars['countmachineunit'])
    intCountMachine = int(request.vars['countmachine'])
    intCountMachineActivity = int(request.vars['countmachineactivity'])
    intCountDailySchedule = int(request.vars['countdailyschedule'])
    intCountArticle = int(request.vars['countarticle'])
    intCountOrder = int(request.vars['countorder'])
    intCountShiftTime = int(request.vars['countshifttime'])
    intCountTeam = int(request.vars['countteam'])
    intCountReg = int(request.vars['countreg'])
    intCountProgress = int(request.vars['countprogress'])

    intCount = 0
    intCount = dboee((dboee.tblOee_Country.fldOeeCountryNr == intCountryNr) & \
                     (dboee.tblOee_Country.fldOeeSync == True)).count()
    if intCount > 0:
        intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_Plant.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Plant.fldOeePlantNr == intPlantNr) & \
                         (dboee.tblOee_Plant.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_SubPlant.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_SubPlant.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_SubPlant.fldOeeSubPlantNr == intSubPlantNr) & \
                         (dboee.tblOee_SubPlant.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_Department.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Department.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_Department.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_Department.fldOeeDepartmentNr == intDepartmentNr) & \
                         (dboee.tblOee_Department.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_ActivityGroup.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_ActivityGroup.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_ActivityGroup.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_ActivityGroup.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_ActivityGroup.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_Activity.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Activity.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_Activity.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_Activity.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_Activity.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_ModuleSensorStyle.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_ModuleSensorStyle.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_ModuleType.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_ModuleType.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_ModuleType.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_ModuleType.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_ModuleType.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_Module.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Module.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_Module.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_Module.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_Module.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_MachineIOFailure.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_MachineIOFailure.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_MachineIOFailure.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_MachineIOFailure.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_MachineIOFailure.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_MachineShortbreak.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_MachineShortbreak.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_MachineShortbreak.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_MachineShortbreak.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_MachineShortbreak.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_MachineStatus.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_MachineStatus.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_MachineStatus.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_MachineStatus.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_MachineStatus.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_MachineUndefinedProduction.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_MachineUndefinedProduction.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_MachineUndefinedProduction.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_MachineUndefinedProduction.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_MachineUndefinedProduction.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_MachineUndefinedStandstill.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_MachineUndefinedStandstill.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_MachineUndefinedStandstill.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_MachineUndefinedStandstill.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_MachineUndefinedStandstill.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_MachineUnscheduled.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_MachineUnscheduled.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_MachineUnscheduled.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_MachineUnscheduled.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_MachineUnscheduled.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_MachineUnit.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_MachineUnit.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_MachineUnit.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_MachineUnit.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_MachineUnit.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_Machine.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Machine.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_Machine.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_Machine.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_Machine.fldOeeMachineNr == intMachineID) & \
                         (dboee.tblOee_Machine.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_MachineActivity.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_MachineActivity.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_MachineActivity.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_MachineActivity.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_MachineActivity.fldOeeMachineID == intMachineID) & \
                         (dboee.tblOee_MachineActivity.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_DailySchedule.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_DailySchedule.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_DailySchedule.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_DailySchedule.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_DailySchedule.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_Article.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Article.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_Article.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_Article.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_Article.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_Order.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Order.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_Order.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_Order.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_Order.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_ShiftTime.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_ShiftTime.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_ShiftTime.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_ShiftTime.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_ShiftTime.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    intCount = 0
    if intUpdate == 0:
        intCount = dboee((dboee.tblOee_Team.fldOeeCountryID == intCountryNr) & \
                         (dboee.tblOee_Team.fldOeePlantID == intPlantNr) & \
                         (dboee.tblOee_Team.fldOeeSubPlantID == intSubPlantNr) & \
                         (dboee.tblOee_Team.fldOeeDepartmentID == intDepartmentNr) & \
                         (dboee.tblOee_Team.fldOeeSync == True)).count()
        if intCount > 0:
            intUpdate = 1
    return intUpdate

def user():
    return dict(form=auth(), \
                strLevel='Top', \
                intScreenWidth=1066)

def screenwidth():
    try:
        intScreenWidth = int(request.vars['screenwidth'])
    except:
        intScreenWidth = 0
    if intScreenWidth > 0:
        redirect(URL('index?screenwidth=' + str(intScreenWidth)))
    return intScreenWidth

def colorpicker():
    form = SQLFORM.factory(Field('color_description', 'string'), \
                           Field('color', widget = color_widget.widget))
    if form.process().accepted:
        response.flash = form.vars.color
    return dict(form = form)

def call():
    session.forget()
    return service()
