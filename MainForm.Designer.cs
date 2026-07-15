namespace PetProj
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSaveDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveDocumentAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPrintDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPreviewDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEditMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.отменадействияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отменадействияToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.вырезатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вставкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.выделитьвсеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServiceMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTuningApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAppParameters = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelpContent = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelpIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAboutApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbCreateDocument = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenDocument = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveDocument = new System.Windows.Forms.ToolStripButton();
            this.tsbPrintDocument = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCut = new System.Windows.Forms.ToolStripButton();
            this.tsbCopy = new System.Windows.Forms.ToolStripButton();
            this.tsbPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHelp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbArrow = new System.Windows.Forms.ToolStripButton();
            this.tsbLine = new System.Windows.Forms.ToolStripButton();
            this.tsbRect = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.placeHolder = new System.Windows.Forms.Panel();
            this.timerUpdateControls = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFileMenu,
            this.tsmiEditMenu,
            this.tsmiServiceMenu,
            this.tsmiHelpMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1117, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFileMenu
            // 
            this.tsmiFileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCreateDocument,
            this.tsmiOpenDocument,
            this.toolStripSeparator,
            this.tsmiSaveDocument,
            this.tsmiSaveDocumentAs,
            this.toolStripSeparator1,
            this.tsmiPrintDocument,
            this.tsmiPreviewDocument,
            this.toolStripSeparator2,
            this.tsmiExit});
            this.tsmiFileMenu.Name = "tsmiFileMenu";
            this.tsmiFileMenu.Size = new System.Drawing.Size(48, 20);
            this.tsmiFileMenu.Text = "&Файл";
            // 
            // tsmiCreateDocument
            // 
            this.tsmiCreateDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCreateDocument.Image")));
            this.tsmiCreateDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiCreateDocument.Name = "tsmiCreateDocument";
            this.tsmiCreateDocument.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.tsmiCreateDocument.Size = new System.Drawing.Size(233, 22);
            this.tsmiCreateDocument.Text = "&Создать";
            this.tsmiCreateDocument.Click += new System.EventHandler(this.tsmiCreateDocument_Click);
            // 
            // tsmiOpenDocument
            // 
            this.tsmiOpenDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsmiOpenDocument.Image")));
            this.tsmiOpenDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiOpenDocument.Name = "tsmiOpenDocument";
            this.tsmiOpenDocument.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmiOpenDocument.Size = new System.Drawing.Size(233, 22);
            this.tsmiOpenDocument.Text = "&Открыть";
            this.tsmiOpenDocument.Click += new System.EventHandler(this.tsmiOpenDocument_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(230, 6);
            // 
            // tsmiSaveDocument
            // 
            this.tsmiSaveDocument.Enabled = false;
            this.tsmiSaveDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsmiSaveDocument.Image")));
            this.tsmiSaveDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiSaveDocument.Name = "tsmiSaveDocument";
            this.tsmiSaveDocument.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiSaveDocument.Size = new System.Drawing.Size(233, 22);
            this.tsmiSaveDocument.Text = "&Сохранить";
            this.tsmiSaveDocument.Click += new System.EventHandler(this.tsmiSaveDocument_Click);
            // 
            // tsmiSaveDocumentAs
            // 
            this.tsmiSaveDocumentAs.Enabled = false;
            this.tsmiSaveDocumentAs.Name = "tsmiSaveDocumentAs";
            this.tsmiSaveDocumentAs.Size = new System.Drawing.Size(233, 22);
            this.tsmiSaveDocumentAs.Text = "Сохранить &как";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(230, 6);
            // 
            // tsmiPrintDocument
            // 
            this.tsmiPrintDocument.Enabled = false;
            this.tsmiPrintDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPrintDocument.Image")));
            this.tsmiPrintDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiPrintDocument.Name = "tsmiPrintDocument";
            this.tsmiPrintDocument.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.tsmiPrintDocument.Size = new System.Drawing.Size(233, 22);
            this.tsmiPrintDocument.Text = "&Печать";
            // 
            // tsmiPreviewDocument
            // 
            this.tsmiPreviewDocument.Enabled = false;
            this.tsmiPreviewDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPreviewDocument.Image")));
            this.tsmiPreviewDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiPreviewDocument.Name = "tsmiPreviewDocument";
            this.tsmiPreviewDocument.Size = new System.Drawing.Size(233, 22);
            this.tsmiPreviewDocument.Text = "Предварительный про&смотр";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(230, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(233, 22);
            this.tsmiExit.Text = "Вы&ход";
            // 
            // tsmiEditMenu
            // 
            this.tsmiEditMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.отменадействияToolStripMenuItem,
            this.отменадействияToolStripMenuItem1,
            this.toolStripSeparator3,
            this.вырезатьToolStripMenuItem,
            this.копироватьToolStripMenuItem,
            this.вставкаToolStripMenuItem,
            this.toolStripSeparator4,
            this.выделитьвсеToolStripMenuItem});
            this.tsmiEditMenu.Enabled = false;
            this.tsmiEditMenu.Name = "tsmiEditMenu";
            this.tsmiEditMenu.Size = new System.Drawing.Size(59, 20);
            this.tsmiEditMenu.Text = "&Правка";
            // 
            // отменадействияToolStripMenuItem
            // 
            this.отменадействияToolStripMenuItem.Name = "отменадействияToolStripMenuItem";
            this.отменадействияToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.отменадействияToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.отменадействияToolStripMenuItem.Text = "&Отмена действия";
            // 
            // отменадействияToolStripMenuItem1
            // 
            this.отменадействияToolStripMenuItem1.Name = "отменадействияToolStripMenuItem1";
            this.отменадействияToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.отменадействияToolStripMenuItem1.Size = new System.Drawing.Size(217, 22);
            this.отменадействияToolStripMenuItem1.Text = "&Отмена действия";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(214, 6);
            // 
            // вырезатьToolStripMenuItem
            // 
            this.вырезатьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("вырезатьToolStripMenuItem.Image")));
            this.вырезатьToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.вырезатьToolStripMenuItem.Name = "вырезатьToolStripMenuItem";
            this.вырезатьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.вырезатьToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.вырезатьToolStripMenuItem.Text = "Вырезат&ь";
            // 
            // копироватьToolStripMenuItem
            // 
            this.копироватьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("копироватьToolStripMenuItem.Image")));
            this.копироватьToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.копироватьToolStripMenuItem.Name = "копироватьToolStripMenuItem";
            this.копироватьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.копироватьToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.копироватьToolStripMenuItem.Text = "&Копировать";
            // 
            // вставкаToolStripMenuItem
            // 
            this.вставкаToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("вставкаToolStripMenuItem.Image")));
            this.вставкаToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.вставкаToolStripMenuItem.Name = "вставкаToolStripMenuItem";
            this.вставкаToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.вставкаToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.вставкаToolStripMenuItem.Text = "Вст&авка";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(214, 6);
            // 
            // выделитьвсеToolStripMenuItem
            // 
            this.выделитьвсеToolStripMenuItem.Name = "выделитьвсеToolStripMenuItem";
            this.выделитьвсеToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.выделитьвсеToolStripMenuItem.Text = "Выделить &все";
            // 
            // tsmiServiceMenu
            // 
            this.tsmiServiceMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTuningApplication,
            this.tsmiAppParameters});
            this.tsmiServiceMenu.Enabled = false;
            this.tsmiServiceMenu.Name = "tsmiServiceMenu";
            this.tsmiServiceMenu.Size = new System.Drawing.Size(59, 20);
            this.tsmiServiceMenu.Text = "&Сервис";
            // 
            // tsmiTuningApplication
            // 
            this.tsmiTuningApplication.Name = "tsmiTuningApplication";
            this.tsmiTuningApplication.Size = new System.Drawing.Size(180, 22);
            this.tsmiTuningApplication.Text = "&Настройки";
            // 
            // tsmiAppParameters
            // 
            this.tsmiAppParameters.Name = "tsmiAppParameters";
            this.tsmiAppParameters.Size = new System.Drawing.Size(180, 22);
            this.tsmiAppParameters.Text = "&Параметры";
            // 
            // tsmiHelpMenu
            // 
            this.tsmiHelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelpContent,
            this.tsmiHelpIndex,
            this.tsmiSearch,
            this.toolStripSeparator5,
            this.tsmiAboutApplication});
            this.tsmiHelpMenu.Enabled = false;
            this.tsmiHelpMenu.Name = "tsmiHelpMenu";
            this.tsmiHelpMenu.Size = new System.Drawing.Size(65, 20);
            this.tsmiHelpMenu.Text = "Спра&вка";
            // 
            // tsmiHelpContent
            // 
            this.tsmiHelpContent.Name = "tsmiHelpContent";
            this.tsmiHelpContent.Size = new System.Drawing.Size(180, 22);
            this.tsmiHelpContent.Text = "&Содержание";
            // 
            // tsmiHelpIndex
            // 
            this.tsmiHelpIndex.Name = "tsmiHelpIndex";
            this.tsmiHelpIndex.Size = new System.Drawing.Size(180, 22);
            this.tsmiHelpIndex.Text = "&Индекс";
            // 
            // tsmiSearch
            // 
            this.tsmiSearch.Name = "tsmiSearch";
            this.tsmiSearch.Size = new System.Drawing.Size(180, 22);
            this.tsmiSearch.Text = "&Поиск";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(177, 6);
            // 
            // tsmiAboutApplication
            // 
            this.tsmiAboutApplication.Name = "tsmiAboutApplication";
            this.tsmiAboutApplication.Size = new System.Drawing.Size(180, 22);
            this.tsmiAboutApplication.Text = "&О программе...";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCreateDocument,
            this.tsbOpenDocument,
            this.tsbSaveDocument,
            this.tsbPrintDocument,
            this.toolStripSeparator6,
            this.tsbCut,
            this.tsbCopy,
            this.tsbPaste,
            this.toolStripSeparator7,
            this.tsbHelp,
            this.toolStripSeparator8,
            this.tsbArrow,
            this.tsbLine,
            this.tsbRect});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1117, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbCreateDocument
            // 
            this.tsbCreateDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCreateDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsbCreateDocument.Image")));
            this.tsbCreateDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCreateDocument.Name = "tsbCreateDocument";
            this.tsbCreateDocument.Size = new System.Drawing.Size(23, 22);
            this.tsbCreateDocument.Text = "&Создать";
            this.tsbCreateDocument.Click += new System.EventHandler(this.tsmiCreateDocument_Click);
            // 
            // tsbOpenDocument
            // 
            this.tsbOpenDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpenDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpenDocument.Image")));
            this.tsbOpenDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenDocument.Name = "tsbOpenDocument";
            this.tsbOpenDocument.Size = new System.Drawing.Size(23, 22);
            this.tsbOpenDocument.Text = "&Открыть";
            this.tsbOpenDocument.Click += new System.EventHandler(this.tsmiOpenDocument_Click);
            // 
            // tsbSaveDocument
            // 
            this.tsbSaveDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSaveDocument.Enabled = false;
            this.tsbSaveDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsbSaveDocument.Image")));
            this.tsbSaveDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveDocument.Name = "tsbSaveDocument";
            this.tsbSaveDocument.Size = new System.Drawing.Size(23, 22);
            this.tsbSaveDocument.Text = "&Сохранить";
            this.tsbSaveDocument.Click += new System.EventHandler(this.tsmiSaveDocument_Click);
            // 
            // tsbPrintDocument
            // 
            this.tsbPrintDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrintDocument.Enabled = false;
            this.tsbPrintDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrintDocument.Image")));
            this.tsbPrintDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrintDocument.Name = "tsbPrintDocument";
            this.tsbPrintDocument.Size = new System.Drawing.Size(23, 22);
            this.tsbPrintDocument.Text = "&Печать";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCut
            // 
            this.tsbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCut.Enabled = false;
            this.tsbCut.Image = ((System.Drawing.Image)(resources.GetObject("tsbCut.Image")));
            this.tsbCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCut.Name = "tsbCut";
            this.tsbCut.Size = new System.Drawing.Size(23, 22);
            this.tsbCut.Text = "В&ырезать";
            // 
            // tsbCopy
            // 
            this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopy.Enabled = false;
            this.tsbCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsbCopy.Image")));
            this.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.Size = new System.Drawing.Size(23, 22);
            this.tsbCopy.Text = "&Копировать";
            // 
            // tsbPaste
            // 
            this.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPaste.Enabled = false;
            this.tsbPaste.Image = ((System.Drawing.Image)(resources.GetObject("tsbPaste.Image")));
            this.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPaste.Name = "tsbPaste";
            this.tsbPaste.Size = new System.Drawing.Size(23, 22);
            this.tsbPaste.Text = "Вст&авка";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbHelp
            // 
            this.tsbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbHelp.Enabled = false;
            this.tsbHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsbHelp.Image")));
            this.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHelp.Name = "tsbHelp";
            this.tsbHelp.Size = new System.Drawing.Size(23, 22);
            this.tsbHelp.Text = "Спр&авка";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbArrow
            // 
            this.tsbArrow.Checked = true;
            this.tsbArrow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbArrow.Image = global::PetProj.Properties.Resources.arrow;
            this.tsbArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbArrow.Name = "tsbArrow";
            this.tsbArrow.Size = new System.Drawing.Size(23, 22);
            this.tsbArrow.Text = "Режим выбора";
            this.tsbArrow.Click += new System.EventHandler(this.tsbArrow_Click);
            // 
            // tsbLine
            // 
            this.tsbLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLine.Image = global::PetProj.Properties.Resources.line;
            this.tsbLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLine.Name = "tsbLine";
            this.tsbLine.Size = new System.Drawing.Size(23, 22);
            this.tsbLine.Text = "Построение отрезков";
            this.tsbLine.Click += new System.EventHandler(this.tsbArrow_Click);
            // 
            // tsbRect
            // 
            this.tsbRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRect.Image = global::PetProj.Properties.Resources.rect;
            this.tsbRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRect.Name = "tsbRect";
            this.tsbRect.Size = new System.Drawing.Size(23, 22);
            this.tsbRect.Text = "Построение прямоугольников";
            this.tsbRect.Click += new System.EventHandler(this.tsbArrow_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 603);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1117, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // placeHolder
            // 
            this.placeHolder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.placeHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.placeHolder.Location = new System.Drawing.Point(0, 49);
            this.placeHolder.Margin = new System.Windows.Forms.Padding(0);
            this.placeHolder.Name = "placeHolder";
            this.placeHolder.Size = new System.Drawing.Size(1117, 554);
            this.placeHolder.TabIndex = 3;
            // 
            // timerUpdateControls
            // 
            this.timerUpdateControls.Tick += new System.EventHandler(this.timerUpdateControls_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 625);
            this.Controls.Add(this.placeHolder);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Чертилка";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFileMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateDocument;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenDocument;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveDocument;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveDocumentAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrintDocument;
        private System.Windows.Forms.ToolStripMenuItem tsmiPreviewDocument;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditMenu;
        private System.Windows.Forms.ToolStripMenuItem отменадействияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отменадействияToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem вырезатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вставкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem выделитьвсеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiServiceMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiTuningApplication;
        private System.Windows.Forms.ToolStripMenuItem tsmiAppParameters;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelpMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelpContent;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelpIndex;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiAboutApplication;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbCreateDocument;
        private System.Windows.Forms.ToolStripButton tsbOpenDocument;
        private System.Windows.Forms.ToolStripButton tsbSaveDocument;
        private System.Windows.Forms.ToolStripButton tsbPrintDocument;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbCut;
        private System.Windows.Forms.ToolStripButton tsbCopy;
        private System.Windows.Forms.ToolStripButton tsbPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton tsbHelp;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel placeHolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton tsbArrow;
        private System.Windows.Forms.ToolStripButton tsbLine;
        private System.Windows.Forms.ToolStripButton tsbRect;
        private System.Windows.Forms.Timer timerUpdateControls;
    }
}

