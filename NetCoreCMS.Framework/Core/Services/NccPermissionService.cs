﻿/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.IoC;

namespace NetCoreCMS.Framework.Core.Services
{
    /// <summary>
    /// Service for user authorization policy. 
    /// </summary>
    public class NccPermissionService : IBaseService<NccPermission>, ITransient
    {
        private readonly NccPermissionRepository _entityRepository;

        public NccPermissionService(NccPermissionRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccPermission Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking);
        } 

        public List<NccPermission> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        } 

        public NccPermission Save(NccPermission entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccPermission Update(NccPermission entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
            if (oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(entity, oldEntity);
                    _entityRepository.Edit(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }

            return entity;
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        private void CopyNewData(NccPermission copyFrom, NccPermission copyTo)
        {
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = copyFrom.ModifyBy;
            copyTo.Name = copyFrom.Name;
            copyTo.Status = copyFrom.Status;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.Metadata = copyFrom.Metadata;

            copyTo.Group = copyFrom.Group;
            copyTo.Description = copyFrom.Description;
        }

        public void SaveOrUpdate(NccPermission permission)
        {
            using (var txn = _entityRepository.BeginTransaction()) {

                if (permission.Id > 0)
                {
                    var oldPermission = _entityRepository.Get(permission.Id);
                    if (oldPermission != null)
                    {
                        
                    }
                }
                else
                {
                    _entityRepository.Add(permission);
                }

                txn.Commit();
                _entityRepository.SaveChange();
            }
        }
    }
}