using WinFormsShop.Data.Entities;
using WinFormsShop.Data;
using Microsoft.EntityFrameworkCore.Storage;
using WinFormsShop.Helpers;
using WinFormsShop.Migrations;
using System.Diagnostics.Eventing.Reader;
using EmailSendLib.Abstract;
using EmailSendLib.Sevices;
using Quartz;

namespace WinFormsShop
{
    public partial class CategoryForm : Form
    {
        public MyDataContext context { get; set; }

        public CategoryForm()
        {
            InitializeComponent();
            context = new MyDataContext();
        }
        private void btn_AddCategory_Click(object sender, EventArgs e)
        {
            AddCategoryForm addCategory = new AddCategoryForm();
            if (addCategory.ShowDialog() == DialogResult.OK)
            {
                if (String.IsNullOrWhiteSpace(addCategory.mTitle))
                {
                    MessageBox.Show("Title is empty");
                    return;
                }
                if (isCategoryExistes(addCategory.mTitle))
                {
                    MessageBox.Show("This Category already existes");
                    return;
                }
                if (!addCategory.isPhoto)
                {
                    MessageBox.Show("Choose photo");
                    return;
                }
                CategoryEntity category = new CategoryEntity();
                category.Title = addCategory.mTitle;
                category.Priority = addCategory.mPrioriy;
                category.DateCreated = DateTime.Now;
                Bitmap bmp = new Bitmap(addCategory.mImage);
                String dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images");
                string fileSaveName = System.IO.Path.GetRandomFileName() + ".jpg";
                category.Image = fileSaveName;
                for (int i = 1; i < 6; i++)
                {
                    var bmpSave = ImageWorker.CompressImage(bmp, i * 32, i * 32);
                    string imageSave = System.IO.Path.Combine(dir, $"{i * 32}_" + fileSaveName); 
                    bmpSave.Save(imageSave, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                context.Categories.Add(category);
                context.SaveChanges();
                GetCategories();
                MessageBox.Show("Category has been added!");
            }
        }

        private void btnAddSubCategory_Click(object sender, EventArgs e)
        {
            AddSubCategoryForm addSubCategory = new AddSubCategoryForm();
            if (addSubCategory.ShowDialog() == DialogResult.OK)
            {
                if (addSubCategory.mCategoryId == null)
                {
                    MessageBox.Show("Category Title is empty");
                    return;
                }
                if (String.IsNullOrWhiteSpace(addSubCategory.mSubCategory))
                {
                    MessageBox.Show("Sub-category is empty");
                    return;
                }
                if (!addSubCategory.isPhoto)
                {
                    MessageBox.Show("Choose photo");
                    return;
                }
                if (isSubCategoryExistes(int.Parse(addSubCategory.mCategoryId), addSubCategory.mSubCategory))
                    return;
                
                CategoryEntity subCategory = new CategoryEntity();
                subCategory.ParentId = int.Parse(addSubCategory.mCategoryId);
                subCategory.Title = addSubCategory.mSubCategory;
                subCategory.Priority = addSubCategory.mPriority;
                subCategory.DateCreated = DateTime.Now;
                Bitmap bmp = new Bitmap(addSubCategory.mImage);
                String dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images");
                string fileSaveName = System.IO.Path.GetRandomFileName() + ".jpg";
                subCategory.Image = fileSaveName;
                for (int i = 1; i < 6; i++)
                {
                    var bmpSave = ImageWorker.CompressImage(bmp, i * 32, i * 32);
                    string imageSave = System.IO.Path.Combine(dir, $"{i * 32}_" + fileSaveName);
                    bmpSave.Save(imageSave, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                context.Categories.Add(subCategory);
                context.SaveChanges();
                GetCategories();
                MessageBox.Show("Sub-category has been added");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var select = tvCategories.SelectedNode;
            if (select != null)
            {
                var categoryEdit = (CategoryEntity)select.Tag;
                string oldPhoto = categoryEdit.Image;
                EditCategoryForm editCategoryForm = new EditCategoryForm(categoryEdit);
                if (editCategoryForm.ShowDialog() == DialogResult.OK)
                {
                    if (String.IsNullOrWhiteSpace(editCategoryForm.newTitle))
                    {
                        MessageBox.Show("Category Title is empty");
                        return;
                    }
                    categoryEdit.Title = editCategoryForm.newTitle;
                    categoryEdit.IsDelete = editCategoryForm.newIsDeleted;
                    if (editCategoryForm.newParentId != null)
                        categoryEdit.ParentId = int.Parse(editCategoryForm.newParentId);
                    categoryEdit.Priority = editCategoryForm.newPriority;
                    categoryEdit.DateCreated = editCategoryForm.newDateCreate;
                    Bitmap bmp = new Bitmap(editCategoryForm.newPhoto);
                    String dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images");
                    string fileSaveName = System.IO.Path.GetRandomFileName() + ".jpg";
                    categoryEdit.Image = fileSaveName;
                    for (int i = 1; i < 6; i++)
                    {
                        var bmpSave = ImageWorker.CompressImage(bmp, i * 32, i * 32);
                        string imageSave = System.IO.Path.Combine(dir, $"{i * 32}_" + fileSaveName);
                        bmpSave.Save(imageSave, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    context.Categories.Update(categoryEdit);
                    context.SaveChanges();
                    GetCategories();
                    DeleteOldPhoto(oldPhoto);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var select = tvCategories.SelectedNode;
            if (select != null)
            {
                tvCategories.Nodes.Clear();
                var categoryDelete = (CategoryEntity)select.Tag;
                if (categoryDelete.ParentId == null)
                {
                    foreach (var item in context.Categories.ToList())
                    {
                        if (item.ParentId == categoryDelete.Id)
                        {
                            item.ParentId = null;
                            context.Categories.Update(item);
                            context.SaveChanges();
                        }
                    }
                }
                context.Categories.Remove(categoryDelete);
                context.SaveChanges();
                GetCategories();
                DeleteOldPhoto(categoryDelete.Image);
            }
        }

        private void DeleteOldPhoto(string photoName)
        {
            String dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images");
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles();
            foreach (var file in files)
            {
                if (file.ToString().Contains(photoName))
                {
                    try
                    {
                        File.Delete(file.FullName);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private bool isCategoryExistes(string title)
        {
            if (context.Categories.ToList().Any(x => x.Title == title))
                return true;
            return false;
        }

        private bool isSubCategoryExistes(int categoryId, string title)
        {
            if (context.Categories.ToList().Any(x => x.Title == title && x.ParentId == null))
            {
                MessageBox.Show("Title is occupied by category");
                return true;
            }
            if (context.Categories.ToList().Any(x => x.Title == title && x.ParentId == categoryId))
            {
                MessageBox.Show("This sub-category already existes in this category");
                return true;
            }
            return false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GetCategories();
        }

        private void GetCategories()
        {
            tvCategories.Nodes.Clear();

            tvCategories.ImageList = new ImageList();
            foreach (var item in context.Categories.Where(x => x.ParentId == null).ToList())
            {
                TreeNode node = new TreeNode(item.Title);
                string fileName = Directory.GetCurrentDirectory() + $"\\Images\\32_{item.Image}";
                Stream stream = ConvertFileToByteArrayThenToMemoryStream(fileName);
                tvCategories.ImageList.Images.Add(item.Id.ToString(), Image.FromStream(stream));
                node.ImageKey = item.Id.ToString();
                node.SelectedImageKey = item.Id.ToString();
                node.Tag = item;
                node.Nodes.Add("");
                tvCategories.Nodes.Add(node);
            }
        }

        private Stream ConvertFileToByteArrayThenToMemoryStream(string fileName)
        {
            byte[] byteArray = File.ReadAllBytes(fileName);
            Stream stream = new MemoryStream(byteArray);
            return stream;
        }

        private void tvCategories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var list = context.Categories.ToList();
            var hitTest = e.Node.TreeView.HitTest(e.Location);
            if (hitTest.Location != TreeViewHitTestLocations.PlusMinus) // якщо ти нажав на назву ноди а не на самий плюсік (розширення)
            {
                var item = e.Node.Tag as CategoryEntity;
                string output;
                if (item.ParentId != null)
                    output = $"Id: {item.Id};\nParentId: {item.ParentId};\nDateCreate: {item.DateCreated.ToShortDateString()};\nImage: {item.Image}";
                else
                    output = $"Id: {item.Id};\nDateCreate: {item.DateCreated.ToShortDateString()};\nImage: {item.Image}";
                MessageBox.Show(output, "Category Data", MessageBoxButtons.OK);
                return;
            }

            if (!e.Node.IsExpanded) // якщо ти закриваєш плюсік (розширення)
            {
                e.Node.Nodes.Clear();
                e.Node.Nodes.Add("");
                return; 
            }

            var categoryClick = e.Node.Tag as CategoryEntity;
            var list2 = context.Categories.Where(x => x.ParentId == categoryClick.Id).ToList();
            e.Node.Nodes.Clear();
            if (list2.Count > 0)
            {
                foreach (var item in list2)
                {
                    TreeNode node = new TreeNode(item.Title);
                    tvCategories.ImageList.Images.Add(item.Id.ToString(), Image.FromFile($"Images\\32_{item.Image}"));
                    node.ImageKey = item.Id.ToString();
                    node.SelectedImageKey = item.Id.ToString();
                    node.Tag = item;
                    e.Node.Nodes.Add(node);
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessageForm sendMessageForm = new SendMessageForm();
            if (sendMessageForm.ShowDialog() != DialogResult.OK) return;
            try
            {
                EmailSendLib.Message message = new EmailSendLib.Message();
                message.To = sendMessageForm.mWhom;
                message.Subject = sendMessageForm.mTopic;
                message.Body = sendMessageForm.mMessage;

                IEmailService emailService = new SmtpEmailService();
                emailService.Send(message);
                MessageBox.Show("Sent", "Message Sending", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem send", ex.Message);
            }
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {
            SendSMSForm sendSMSForm = new SendSMSForm();
            if (sendSMSForm.ShowDialog() != DialogResult.OK) return;
            SMSService sms = new SMSService();
            string result = sms.Send(sendSMSForm.mPhone, sendSMSForm.mSMS);
            SentSMS newSMS = new SentSMS() { Phone = sendSMSForm.mPhone, Message = sendSMSForm.mSMS };
            context.SMS.Add(newSMS);
            context.SaveChanges();
            MessageBox.Show(result);
        }
    }
}