using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolNotification.Models;
using AutoMapper;
using System.Web.Mvc;

namespace SchoolNotification.ViewModels
{
    public class RepoMessage : RepositoryBase
    {
        //Create method
        public MessageFull CreateMessage(MessageCreate newItem)
        {
            Models.Message message = Mapper.Map<Models.Message>(newItem);
            dc.Messages.Add(message);
            dc.SaveChanges();
            return Mapper.Map<MessageFull>(message);
        }

        //Edit method
        public MessageFull EditMessage(MessageFull editItem)
        {
            var msgToEdit = dc.Messages.Find(editItem.Id);
            if (msgToEdit == null) return null;
            else
            {
                dc.Entry(msgToEdit).CurrentValues.SetValues(editItem);
                dc.SaveChanges();
            }
            return Mapper.Map<MessageFull>(msgToEdit);
        }
        //Delete method
        public void DeleteMessage(int? id)
        {
            var msgToDel = dc.Messages.Find(id);
            if (msgToDel == null) return;
            else
            {
                try
                {
                    dc.Messages.Remove(msgToDel);
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        //Get single ViewModel method
        public MessageFull getMessageFull(int? id)
        {
            var msg = dc.Messages.FirstOrDefault(n => n.Id == id);
            if (msg == null) return null;
            try {
                var temp = Mapper.Map<MessageFull>(msg);
                return temp;
            } catch (Exception e){
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return null;
            }
            
        }

        //Get list of ViewModels method
        public IEnumerable<MessageBase> getListOfMessageBase()
        {
            var msgs = dc.Messages.OrderBy(n => n.Id);
            if (msgs == null) return null;
            try
            {
                return Mapper.Map<IEnumerable<MessageBase>>(msgs);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return null;
            }
        }

        //Get list of ViewModels method for given cancellation
        public IEnumerable<MessageBase> getListOfMessageForFaculty(int? facId)
        {
            var cancellations = dc.Cancellations.Include("Message").OrderBy(n => n.Faculty.UserId == facId);
            List<Message> msglist = new List<Message>();
            foreach (var i in cancellations)
            {
                Message msgtemp = new Message();
                msgtemp = i.Message;
                msglist.Add(msgtemp);

            }
            return Mapper.Map<IEnumerable<MessageBase>>(msglist);
        }

    }
}